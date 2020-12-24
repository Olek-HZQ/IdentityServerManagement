using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientPostLogoutRedirectUriMap : BaseEntityTypeConfiguration<ClientPostLogoutRedirectUri>
    {
        public override void Configure(EntityTypeBuilder<ClientPostLogoutRedirectUri> builder)
        {
            builder.ToTable(TableNameConstant.ClientPostLogoutRedirectUri);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.PostLogoutRedirectUris)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
