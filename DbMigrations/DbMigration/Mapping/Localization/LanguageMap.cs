using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.Localization
{
    public class LanguageMap : BaseEntityTypeConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(TableNameConstant.Language);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LanguageCulture).HasMaxLength(20).IsRequired();
            builder.Property(x => x.UniqueSeoCode).HasMaxLength(2);

            base.Configure(builder);
        }
    }
}
