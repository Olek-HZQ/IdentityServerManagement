using IdentityServer.Admin.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.PersistedGrant
{
    public class PersistedGrantMap : BaseEntityTypeConfiguration<IdentityServer.Admin.Core.Entities.PersistedGrant>
    {
        public override void Configure(EntityTypeBuilder<IdentityServer.Admin.Core.Entities.PersistedGrant> builder)
        {
            builder.ToTable(TableNameConstant.PersistedGrant);
            builder.HasKey(x => x.Key);

            builder.Property(x => x.Key).HasMaxLength(200).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Type).HasMaxLength(50).IsRequired();
            builder.Property(x => x.SubjectId).HasMaxLength(200);
            builder.Property(x => x.SessionId).HasMaxLength(100);
            builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Data).IsRequired();

            base.Configure(builder);
        }
    }
}
