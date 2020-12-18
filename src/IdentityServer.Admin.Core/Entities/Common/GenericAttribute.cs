using System;
using Dapper.Contrib.Extensions;

namespace IdentityServer.Admin.Core.Entities.Common
{
    [Table(nameof(GenericAttribute))]
    public class GenericAttribute
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the key group
        /// </summary>
        public string KeyGroup { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the created or updated date
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
