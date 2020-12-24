using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientRedirectUriMap : BaseEntityTypeConfiguration<ClientRedirectUri>
    {
        public override void Configure(EntityTypeBuilder<ClientRedirectUri> builder)
        {
            builder.ToTable(TableNameConstant.ClientRedirectUri);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.RedirectUris)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
