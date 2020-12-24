using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientIdPRestrictionMap : BaseEntityTypeConfiguration<ClientIdPRestriction>
    {
        public override void Configure(EntityTypeBuilder<ClientIdPRestriction> builder)
        {
            builder.ToTable(TableNameConstant.ClientIdPRestriction);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Provider).HasMaxLength(200).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.IdentityProviderRestrictions)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
