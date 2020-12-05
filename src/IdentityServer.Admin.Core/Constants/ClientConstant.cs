using System.Collections.Generic;

namespace IdentityServer.Admin.Core.Constants
{
    public static class ClientConstant
    {
        public static List<string> GetSecretTypes()
        {
            return new List<string>()
            {
                "SharedSecret",
                "X509Thumbprint",
                "X509Name",
                "X509CertificateBase64"
            };
        }

        public static List<string> GetStandardClaims()
        {
            return new List<string>()
            {
                "name",
                "given_name",
                "family_name",
                "middle_name",
                "nickname",
                "preferred_username",
                "profile",
                "picture",
                "website",
                "gender",
                "birthdate",
                "zoneinfo",
                "locale",
                "address",
                "updated_at"
            };
        }

        public static List<string> GetGrantTypes()
        {
            return new List<string>()
            {
                "implicit",
                "client_credentials",
                "authorization_code",
                "hybrid",
                "password",
                "urn:ietf:params:oauth:grant-type:device_code",
                "delegation"
            };
        }
    }
}
