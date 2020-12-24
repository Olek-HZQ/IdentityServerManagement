using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Localization
{
    public class LocaleStringResourceMap:BaseEntityTypeConfiguration<LocaleStringResource>
    {
        public override void Configure(EntityTypeBuilder<LocaleStringResource> builder)
        {
            builder.ToTable(TableNameConstant.LocaleStringResource);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ResourceName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ResourceValue).IsRequired();

            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
