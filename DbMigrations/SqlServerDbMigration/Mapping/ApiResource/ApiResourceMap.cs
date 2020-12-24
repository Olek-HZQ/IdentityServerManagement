using IdentityServer.Admin.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiResource
{
    public class ApiResourceMap:BaseEntityTypeConfiguration<IdentityServer.Admin.Core.Entities.ApiResource.ApiResource>
    {
        public override void Configure(EntityTypeBuilder<IdentityServer.Admin.Core.Entities.ApiResource.ApiResource> builder)
        {
            builder.ToTable(TableNameConstant.ApiResource);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.DisplayName).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.Property(x => x.AllowedAccessTokenSigningAlgorithms).HasMaxLength(100);

            base.Configure(builder);
        }
    }
}
