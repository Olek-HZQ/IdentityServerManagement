namespace IdentityServer.Admin.Core.Constants
{
    /// <summary>
    /// 定义表名常量（用于Table标签和ef迁移生成数据库时配置的表名约束）
    /// </summary>
    public class TableNameConstant
    {
        #region ApiResource

        public const string ApiResource = "ApiResources";

        public const string ApiResourceClaim = "ApiResourceClaims";

        public const string ApiResourceProperty = "ApiResourceProperties";

        public const string ApiResourceScope = "ApiResourceScopes";

        public const string ApiResourceSecret = "ApiResourceSecrets";

        #endregion

        #region ApiScope

        public const string ApiScope = "ApiScopes";

        public const string ApiScopeClaim = "ApiScopeClaims";

        public const string ApiScopeProperty = "ApiScopeProperties";

        #endregion

        #region Client

        public const string Client = "Clients";

        public const string ClientClaim = "ClientClaims";

        public const string ClientCorsOrigin = "ClientCorsOrigins";

        public const string ClientGrantType = "ClientGrantTypes";

        public const string ClientIdPRestriction = "ClientIdPRestrictions";

        public const string ClientPostLogoutRedirectUri = "ClientPostLogoutRedirectUris";

        public const string ClientProperty = "ClientProperties";

        public const string ClientRedirectUri = "ClientRedirectUris";

        public const string ClientScope = "ClientScopes";

        public const string ClientSecret = "ClientSecrets";

        #endregion

        #region Common

        public const string GenericAttribute = "GenericAttribute";

        #endregion

        #region IdentityResource

        public const string IdentityResource = "IdentityResources";

        public const string IdentityResourceClaim = "IdentityResourceClaims";

        public const string IdentityResourceProperty = "IdentityResourceProperties";

        #endregion

        #region Localization

        public const string Language = "Language";

        public const string LocaleStringResource = "LocaleStringResource";

        #endregion

        #region User and role

        public const string Role = "Roles";

        public const string User = "Users";

        public const string UserClaim = "UserClaims";

        public const string UserPassword = "UserPasswords";

        public const string UserRoleMap = "UserRoleMap";

        #endregion

        #region PersistedGrant

        public const string PersistedGrant = "PersistedGrants";

        #endregion
    }
}
