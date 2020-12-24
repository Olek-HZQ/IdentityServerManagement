using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.ApiResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiResource
{
    public class ApiResourceSecretMap:BaseEntityTypeConfiguration<ApiResourceSecret>
    {
        public override void Configure(EntityTypeBuilder<ApiResourceSecret> builder)
        {
            builder.ToTable(TableNameConstant.ApiResourceSecret);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(200).IsRequired();

            builder.HasOne(x => x.ApiResource)
                .WithMany(x => x.Secrets)
                .HasForeignKey(x => x.ApiResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
