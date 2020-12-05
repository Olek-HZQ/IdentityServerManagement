using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Services.Security;
using IdentityServer.Admin.Services.Stores;
using IdentityServer.AuthIdentity.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace IdentityServer.AuthIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnvironment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews();

            var dbConnectionConfig = Configuration.GetSection(nameof(DbConnectionConfiguration)).Get<DbConnectionConfiguration>();
            services.AddSingleton(dbConnectionConfig);
            var adminConfiguration = Configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();
            services.AddSingleton(adminConfiguration);

            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration =>
            {
                //register all available validators from Nop assemblies
                var assemblies = mvcBuilder.PartManager.ApplicationParts
                    .OfType<AssemblyPart>()
                    .Where(part => part.Name.StartsWith("IdentityServer.AuthIdentity", StringComparison.InvariantCultureIgnoreCase))
                    .Select(part => part.Assembly);
                configuration.RegisterValidatorsFromAssemblies(assemblies);

                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

            var identityServerBuilder = services.AddIdentityServer()
                .AddClientStore<ClientStore>()
                .AddResourceStore<ResourceStore>()
                .AddCorsPolicyService<CorsPolicyService>()
                .AddProfileService<ProfileService>()
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            var certificateConfiguration = Configuration.GetSection(nameof(CertificateConfiguration)).Get<CertificateConfiguration>();

            if (certificateConfiguration.UseTemporarySigningKeyForDevelopment)
            {
                identityServerBuilder.AddDeveloperSigningCredential(true, certificateConfiguration.TemporaryCertificateFileName);
            }
            else if (certificateConfiguration.UseSigningCertificatePfxFile)
            {
                identityServerBuilder.AddSigningCredential(new X509Certificate2(
                    System.IO.Path.Combine(HostEnvironment.ContentRootPath, certificateConfiguration.SigningCertificatePfxFilePath),
                    certificateConfiguration.SigningCertificatePfxFilePassword));
            }
            else
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }

            var dataProtectionKeysPath = CommonHelper.MapPath(HostEnvironment, adminConfiguration.DataProtectionPath);
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(dataProtectionKeysPath))
                .SetDefaultKeyLifetime(TimeSpan.FromDays(60));

            var healthChecksBuilder = services.AddHealthChecks();

            var currentDataProviderType =
                Enum.IsDefined(typeof(DataProviderType), dbConnectionConfig.CurrentDataProviderType)
                    ? dbConnectionConfig.CurrentDataProviderType
                    : DataProviderType.SqlServer;

            // Add HSTS options
            RegisterHstsOptions(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            switch (currentDataProviderType)
            {
                // case DataProviderType.SqlServer: or default
                default:
                    healthChecksBuilder.AddSqlServer(dbConnectionConfig.MasterSqlServerConnString);

                    builder.RegisterAssemblyTypes(Assembly.Load("IdentityServer.Admin.Dapper"))
                        .Where(x => x.Namespace != null && x.Namespace.StartsWith($"IdentityServer.Admin.Dapper.Repositories.{DataProviderType.SqlServer}")
                                                        && x.Name.EndsWith("Repository"))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

                    builder.RegisterAssemblyTypes(Assembly.Load("IdentityServer.Admin.Services"))
                        .Where(x => x.Namespace != null && x.Namespace.StartsWith($"IdentityServer.Admin.Services.{DataProviderType.SqlServer}")
                                                        && x.Name.EndsWith("Service"))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

                    break;
                case DataProviderType.Mysql:
                    healthChecksBuilder.AddMySql(dbConnectionConfig.MasterSqlServerConnString);
                    break;
                case DataProviderType.Oracle:
                    healthChecksBuilder.AddOracle(dbConnectionConfig.MasterSqlServerConnString);
                    break;
            }

            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();

            return new AutofacServiceProvider(builder.Build());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            UseSecurityHeaders(app);

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoint.MapDefaultControllerRoute();
            });
        }

        private static void RegisterHstsOptions(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }

        /// <summary>
        /// Using of Forwarded Headers and Referrer Policy
        /// </summary>
        /// <param name="app"></param>
        private void UseSecurityHeaders(IApplicationBuilder app)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.All
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);

            app.UseReferrerPolicy(options => options.NoReferrer());

            // CSP Configuration to be able to use external resources
            var cspTrustedDomains = new List<string>();
            Configuration.GetSection("CspTrustedDomains").Bind(cspTrustedDomains);
            if (cspTrustedDomains.Any())
            {
                app.UseCsp(csp =>
                {
                    csp.ImageSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                    });
                    csp.FontSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                    });
                    csp.ScriptSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                        options.UnsafeInlineSrc = true;
                    });
                    csp.StyleSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                        options.UnsafeInlineSrc = true;
                    });
                    csp.DefaultSources(options =>
                    {
                        options.SelfSrc = true;
                        options.CustomSources = cspTrustedDomains;
                        options.Enabled = true;
                    });
                });
            }
        }
    }
}
