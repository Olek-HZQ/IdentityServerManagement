using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.ApiResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiResource
{
    public class ApiResourcePropertyMap:BaseEntityTypeConfiguration<ApiResourceProperty>
    {
        public override void Configure(EntityTypeBuilder<ApiResourceProperty> builder)
        {
            builder.ToTable(TableNameConstant.ApiResourceProperty);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();

            builder.HasOne(x => x.ApiResource)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.ApiResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
