using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Common
{
    public class GenericAttributeMap : BaseEntityTypeConfiguration<GenericAttribute>
    {
        public override void Configure(EntityTypeBuilder<GenericAttribute> builder)
        {
            builder.ToTable(TableNameConstant.GenericAttribute);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.KeyGroup).HasMaxLength(400).IsRequired();
            builder.Property(x => x.Key).HasMaxLength(400).IsRequired();
            builder.Property(x => x.Value).IsRequired();

            base.Configure(builder);
        }
    }
}
