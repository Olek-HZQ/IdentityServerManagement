using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Users
{
    public class UserMap : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableNameConstant.User);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SubjectId).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(64).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(256).IsRequired();

            base.Configure(builder);
        }
    }
}
