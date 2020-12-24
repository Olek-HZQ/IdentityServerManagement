using IdentityServer.Admin.Core.Constants;
using IdentityServer.Admin.Core.Entities.ApiScope;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbMigration.Mapping.ApiScope
{
    public class ApiScopeClaimMap:BaseEntityTypeConfiguration<ApiScopeClaim>
    {
        public override void Configure(EntityTypeBuilder<ApiScopeClaim> builder)
        {
            builder.ToTable(TableNameConstant.ApiScopeClaim);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).HasMaxLength(200).IsRequired();

            builder.HasOne(x => x.ApiScope)
                .WithMany(x => x.UserClaims)
                .HasForeignKey(x => x.ScopeId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
