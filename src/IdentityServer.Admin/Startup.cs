using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using IdentityServer.Admin.Core;
using IdentityServer.Admin.Core.Configuration;
using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.ExceptionHandling;
using IdentityServer.Admin.Helpers;
using IdentityServer.Admin.SeedData;
using IdentityServer.Admin.Services.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Admin
{
    public class Startup
    {
        public Startup(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            HostEnvironment = hostEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnvironment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews();

            var dbConnectionConfig = Configuration.GetSection(nameof(DbConnectionConfiguration)).Get<DbConnectionConfiguration>();
            services.AddSingleton(dbConnectionConfig);
            var adminConfiguration = Configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();
            services.AddSingleton(adminConfiguration);
            services.AddSingleton(Configuration.GetSection(nameof(IdentityDataConfiguration)).Get<IdentityDataConfiguration>());
            services.AddSingleton(Configuration.GetSection(nameof(IdentityServerDataConfiguration)).Get<IdentityServerDataConfiguration>());

            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration =>
            {
                //register all available validators from Nop assemblies
                var assemblies = mvcBuilder.PartManager.ApplicationParts
                    .OfType<AssemblyPart>()
                    .Where(part => part.Name.StartsWith("IdentityServer.Admin", StringComparison.InvariantCultureIgnoreCase))
                    .Select(part => part.Assembly);
                configuration.RegisterValidatorsFromAssemblies(assemblies);

                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

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

            services.AddScoped<ControllerExceptionFilterAttribute>();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";

                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                    options => { options.Cookie.Name = adminConfiguration.IdentityAdminCookieName; })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = adminConfiguration.IdentityServerBaseUrl;
                    options.RequireHttpsMetadata = adminConfiguration.RequireHttpsMetadata;
                    options.ClientId = adminConfiguration.ClientId;
                    options.ClientSecret = adminConfiguration.ClientSecret;
                    options.ResponseType = adminConfiguration.OidcResponseType;

                    options.Scope.Clear();
                    if (adminConfiguration.Scopes.Any())
                    {
                        foreach (string scope in adminConfiguration.Scopes)
                        {
                            options.Scope.Add(scope);
                        }
                    }

                    options.ClaimActions.MapJsonKey(adminConfiguration.TokenValidationClaimRole, adminConfiguration.TokenValidationClaimRole, adminConfiguration.TokenValidationClaimRole);

                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = adminConfiguration.TokenValidationClaimName,
                        RoleClaimType = adminConfiguration.TokenValidationClaimRole
                    };
                    options.Events = new OpenIdConnectEvents
                    {
                        OnRemoteFailure = context =>
                        {
                            context.Response.Redirect("/");
                            context.HandleResponse();

                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context => OnMessageReceived(context, adminConfiguration),
                        OnRedirectToIdentityProvider = context => OnRedirectToIdentityProvider(context, adminConfiguration)
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConstant.AdministrationPolicy,
                    policy => policy.RequireRole(adminConfiguration.AdministrationRole));
            });

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

                    // Register mysql if you need

                    break;
                case DataProviderType.Oracle:
                    healthChecksBuilder.AddOracle(dbConnectionConfig.MasterSqlServerConnString);

                    // Register oracle if you need

                    break;
            }

            #region Register Common

            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();

            #endregion

            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            // Add custom security headers
            UseSecurityHeaders(app);

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();

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
        }

        private static Task OnMessageReceived(MessageReceivedContext context, AdminConfiguration adminConfiguration)
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(adminConfiguration.IdentityAdminCookieExpiresUtcHours));

            return Task.CompletedTask;
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext context, AdminConfiguration adminConfiguration)
        {
            context.ProtocolMessage.RedirectUri = adminConfiguration.IdentityAdminRedirectUri;

            return Task.CompletedTask;
        }
    }
}
