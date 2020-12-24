using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DbMigration.Factories
{
    public class MySqlDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
    {
        public MySqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();

            // 自己修改数据库连接串
            optionsBuilder.UseMySql("server=127.0.0.1;database=IdentityServer4.Admin.Demo;uid=root;password=123456;Allow User Variables=True;");

            return new MySqlDbContext(optionsBuilder.Options);
        }
    }
}
