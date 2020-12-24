using Dapper.Contrib.Extensions;
using IdentityServer.Admin.Core.Constants;

namespace IdentityServer.Admin.Core.Entities.Localization
{
    [Table(TableNameConstant.LocaleStringResource)]
    public class LocaleStringResource
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the resource name
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource value
        /// </summary>
        public string ResourceValue { get; set; }

        /// <summary>
        /// 此用为配置ef迁移时生成外键，故dapper不需要映射该对象
        /// </summary>
        [Computed]
        public virtual Language Language { get; set; }
    }
}
