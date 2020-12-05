using Newtonsoft.Json;

namespace IdentityServer.Admin.Core.Extensions
{
    public static class JsonExtension
    {
        public static string Serialize<T>(this T obj, JsonSerializerSettings settings = null) where T : class
        {
            if (obj == null)
                return string.Empty;

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T Deserialize<T>(this string jsonStr, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return default;

            return JsonConvert.DeserializeObject<T>(jsonStr, settings);
        }
    }
}
