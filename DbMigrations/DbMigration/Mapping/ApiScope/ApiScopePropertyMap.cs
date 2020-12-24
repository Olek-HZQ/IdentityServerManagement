using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.ApiScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiScope
{
    public class ApiScopePropertyMap : BaseEntityTypeConfiguration<ApiScopeProperty>
    {
        public override void Configure(EntityTypeBuilder<ApiScopeProperty> builder)
        {
            builder.ToTable(TableNameConstant.ApiScopeProperty);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();

            builder.HasOne(x => x.ApiScope)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.ScopeId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
