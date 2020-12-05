using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Admin.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Maps a virtual path to a physical disk path.
        ///<param name="hostEnvironment">hostEnvironment</param>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\web\wwwroot\bin"</returns>
        /// </summary>
        public static string MapPath(IHostEnvironment hostEnvironment, string path)
        {
            var baseDirectory = hostEnvironment.ContentRootPath ?? string.Empty;
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);

            path = path.Replace("~/", string.Empty).TrimStart('/');

            //if virtual path has slash on the end, it should be after transform the virtual path to physical path too
            var pathEnd = path.EndsWith('/') ? Path.DirectorySeparatorChar.ToString() : string.Empty;

            return Combine(baseDirectory, path) + pathEnd;
        }

        /// <summary>
        /// Combines an array of strings into a path
        /// </summary>
        /// <param name="paths">An array of parts of the path</param>
        /// <returns>The combined paths</returns>
        private static string Combine(params string[] paths)
        {
            var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

            if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
                //add leading slash to correctly form path in the UNIX system
                path = "/" + path;

            return path;
        }

        /// <summary>
        /// Determines if the string is a valid Universal Naming Convention (UNC)
        /// for a server and share path.
        /// </summary>
        /// <param name="path">The path to be tested.</param>
        /// <returns><see langword="true"/> if the path is a valid UNC path; 
        /// otherwise, <see langword="false"/>.</returns>
        private static bool IsUncPath(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out Uri uri) && uri.IsUnc;
        }
    }
}
