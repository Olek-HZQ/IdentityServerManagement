using IdentityServer.Admin.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.IdentityResource
{
    public class IdentityResourceMap : BaseEntityTypeConfiguration<IdentityServer.Admin.Core.Entities.IdentityResource.IdentityResource>
    {
        public override void Configure(EntityTypeBuilder<IdentityServer.Admin.Core.Entities.IdentityResource.IdentityResource> builder)
        {
            builder.ToTable(TableNameConstant.IdentityResource);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DisplayName).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);

            base.Configure(builder);
        }
    }
}
