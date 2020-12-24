using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.ApiResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiResource
{
    public class ApiResourceClaimMap:BaseEntityTypeConfiguration<ApiResourceClaim>
    {
        public override void Configure(EntityTypeBuilder<ApiResourceClaim> builder)
        {
            builder.ToTable(TableNameConstant.ApiResourceClaim);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(200).IsRequired();

            builder.HasOne(x => x.ApiResource)
                .WithMany(x=>x.UserClaims)
                .HasForeignKey(x => x.ApiResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
