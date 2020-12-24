using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.IdentityResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.IdentityResource
{
    public class IdentityResourceClaimMap : BaseEntityTypeConfiguration<IdentityResourceClaim>
    {
        public override void Configure(EntityTypeBuilder<IdentityResourceClaim> builder)
        {
            builder.ToTable(TableNameConstant.IdentityResourceClaim);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(200).IsRequired();

            builder.HasOne(x => x.IdentityResource)
                .WithMany(x => x.UserClaims)
                .HasForeignKey(x => x.IdentityResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
