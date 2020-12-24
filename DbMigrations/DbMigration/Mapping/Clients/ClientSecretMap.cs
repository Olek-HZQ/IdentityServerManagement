using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientSecretMap : BaseEntityTypeConfiguration<ClientSecret>
    {
        public override void Configure(EntityTypeBuilder<ClientSecret> builder)
        {
            builder.ToTable(TableNameConstant.ClientSecret);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.Value).HasMaxLength(4000).IsRequired();
            builder.Property(x => x.Type).HasMaxLength(250).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.ClientSecrets)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
