using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.ApiResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiResource
{
    public class ApiResourceScopeMap:BaseEntityTypeConfiguration<ApiResourceScope>
    {
        public override void Configure(EntityTypeBuilder<ApiResourceScope> builder)
        {
            builder.ToTable(TableNameConstant.ApiResourceScope);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Scope).HasMaxLength(20).IsRequired();

            builder.HasOne(x => x.ApiResource)
                .WithMany(x => x.Scopes)
                .HasForeignKey(x => x.ApiResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
