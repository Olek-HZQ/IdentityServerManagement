using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Clients
{
    public class ClientPropertyMap : BaseEntityTypeConfiguration<ClientProperty>
    {
        public override void Configure(EntityTypeBuilder<ClientProperty> builder)
        {
            builder.ToTable(TableNameConstant.ClientProperty);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(2000).IsRequired();

            builder.HasOne(x => x.Client)
                .WithMany(x => x.Properties)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
