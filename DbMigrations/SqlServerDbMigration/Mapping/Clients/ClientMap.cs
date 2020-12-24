using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientMap : BaseEntityTypeConfiguration<Client>
    {
        public override void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(TableNameConstant.Client);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ClientName).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.ClientUri).HasMaxLength(2000);
            builder.Property(x => x.LogoUri).HasMaxLength(2000);
            builder.Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
            builder.Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
            builder.Property(x => x.AllowedIdentityTokenSigningAlgorithms).HasMaxLength(100);
            builder.Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
            builder.Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);

            base.Configure(builder);
        }
    }
}
