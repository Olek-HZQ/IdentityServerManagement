using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Users
{
    public class RoleMap : BaseEntityTypeConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(TableNameConstant.Role);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(64).IsRequired();
            builder.Property(x => x.SystemName).HasMaxLength(64).IsRequired();

            base.Configure(builder);
        }
    }
}
