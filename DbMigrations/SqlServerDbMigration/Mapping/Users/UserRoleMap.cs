using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Users
{
    public class UserRoleMap : BaseEntityTypeConfiguration<UserRoleMapping>
    {
        public override void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.ToTable(TableNameConstant.UserRoleMap);
            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoleMaps)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoleMaps)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
