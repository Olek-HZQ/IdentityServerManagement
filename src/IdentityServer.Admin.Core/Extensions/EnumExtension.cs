using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Admin.Core.Extensions
{
    public class EnumExtension
    {
        public static Dictionary<short, string> ToDictionary<T>() where T : struct, IComparable
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(x => Convert.ToInt16(x), y => y.ToString());
        }
    }
}
