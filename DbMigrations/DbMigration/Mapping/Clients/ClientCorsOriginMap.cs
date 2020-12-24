using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientCorsOriginMap : BaseEntityTypeConfiguration<ClientCorsOrigin>
    {
        public override void Configure(EntityTypeBuilder<ClientCorsOrigin> builder)
        {
            builder.ToTable(TableNameConstant.ClientCorsOrigin);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Origin).HasMaxLength(150).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.AllowedCorsOrigins)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
