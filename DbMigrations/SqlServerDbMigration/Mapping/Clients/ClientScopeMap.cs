using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientScopeMap : BaseEntityTypeConfiguration<ClientScope>
    {
        public override void Configure(EntityTypeBuilder<ClientScope> builder)
        {
            builder.ToTable(TableNameConstant.ClientScope);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Scope).HasMaxLength(200).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.AllowedScopes)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
