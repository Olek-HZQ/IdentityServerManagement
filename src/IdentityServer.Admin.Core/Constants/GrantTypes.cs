using System.Collections.Generic;

namespace IdentityServer.Admin.Core.Constants
{
  public class GrantTypes
  {
    public static ICollection<string> Implicit
    {
      get
      {
        return new[]
        {
            "implicit"
        };
      }
    }

    public static ICollection<string> ImplicitAndClientCredentials
    {
      get
      {
        return new[]
        {
            "implicit",
            "client_credentials"
        };
      }
    }

    public static ICollection<string> Code
    {
      get
      {
        return new[]
        {
            "authorization_code"
        };
      }
    }

    public static ICollection<string> CodeAndClientCredentials
    {
      get
      {
        return new[]
        {
            "authorization_code",
            "client_credentials"
        };
      }
    }

    public static ICollection<string> Hybrid
    {
      get
      {
        return new[]
        {
            "hybrid"
        };
      }
    }

    public static ICollection<string> HybridAndClientCredentials
    {
      get
      {
        return new[]
        {
            "hybrid",
            "client_credentials"
        };
      }
    }

    public static ICollection<string> ClientCredentials
    {
      get
      {
        return new[]
        {
            "client_credentials"
        };
      }
    }

    public static ICollection<string> ResourceOwnerPassword
    {
      get
      {
        return new[]
        {
            "password"
        };
      }
    }

    public static ICollection<string> ResourceOwnerPasswordAndClientCredentials
    {
      get
      {
        return new[]
        {
            "password",
            "client_credentials"
        };
      }
    }

    public static ICollection<string> DeviceFlow
    {
      get
      {
        return new[]
        {
            "urn:ietf:params:oauth:grant-type:device_code"
        };
      }
    }
  }
}
