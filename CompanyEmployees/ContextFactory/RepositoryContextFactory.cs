using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

using Repository;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        //var builder = new DbContextOptionsBuilder<RepositoryContext>()
        //    .UseSqlServer(configuration.GetConnectionString("sqlConnection"));

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(
                connectionString: configuration.GetConnectionString("sqlConnection"),
                sqlServerOptionsAction: b => b.MigrationsAssembly("CompanyEmployees"));


        return new RepositoryContext(builder.Options);
    }
}
