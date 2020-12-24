using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientClaimMap : BaseEntityTypeConfiguration<ClientClaim>
    {
        public override void Configure(EntityTypeBuilder<ClientClaim> builder)
        {
            builder.ToTable(TableNameConstant.ClientClaim);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(250).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
