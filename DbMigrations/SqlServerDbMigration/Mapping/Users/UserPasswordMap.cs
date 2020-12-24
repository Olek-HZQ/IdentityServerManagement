using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Users
{
    public class UserPasswordMap : BaseEntityTypeConfiguration<UserPassword>
    {
        public override void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.ToTable(TableNameConstant.UserPassword);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Password).HasMaxLength(512).IsRequired();
            builder.Property(x => x.PasswordSalt).HasMaxLength(64).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
