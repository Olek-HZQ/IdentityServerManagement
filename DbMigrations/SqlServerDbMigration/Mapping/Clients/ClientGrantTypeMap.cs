using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientGrantTypeMap : BaseEntityTypeConfiguration<ClientGrantType>
    {
        public override void Configure(EntityTypeBuilder<ClientGrantType> builder)
        {
            builder.ToTable(TableNameConstant.ClientGrantType);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GrantType).HasMaxLength(250).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.AllowedGrantTypes)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
