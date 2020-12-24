using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DbMigration.Factories
{
    public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
    {
        public SqlServerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();

            // 自己修改数据库连接串
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=IdentityServer4.Admin;Integrated Security=True;");

            return new SqlServerDbContext(optionsBuilder.Options);
        }
    }
}
