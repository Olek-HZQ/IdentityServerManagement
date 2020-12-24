using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Users
{
    public class UserClaimMap : BaseEntityTypeConfiguration<UserClaim>
    {
        public override void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable(TableNameConstant.UserClaim);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClaimType).HasMaxLength(512).IsRequired();
            builder.Property(x => x.ClaimValue).HasMaxLength(512).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
