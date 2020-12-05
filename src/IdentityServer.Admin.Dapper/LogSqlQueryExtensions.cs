using System.Text;
using Serilog;
using SqlKata;

namespace IdentityServer.Admin.Dapper
{
    public static class LogSqlQueryExtensions
    {
        public static void LogQuerySql(string methodName, SqlResult sqlResult)
        {
            StringBuilder sb = new StringBuilder(50);

            foreach ((string key, object value) in sqlResult.NamedBindings)
            {
                sb.Append($"Key: {key}  Value: {value}\r\n");
            }

            Log.Information($"{methodName} Query Sql:\r\n{sqlResult.Sql}\r\n" + (!string.IsNullOrEmpty(sb.ToString()) ? $"{sb}\r\n" : "\r\n"));
        }
    }
}
