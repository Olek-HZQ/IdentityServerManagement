using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.IdentityResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.IdentityResource
{
    public class IdentityResourcePropertyMap : BaseEntityTypeConfiguration<IdentityResourceProperty>
    {
        public override void Configure(EntityTypeBuilder<IdentityResourceProperty> builder)
        {
            builder.ToTable(TableNameConstant.IdentityResourceProperty);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();

            builder.HasOne(x => x.IdentityResource)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.IdentityResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
