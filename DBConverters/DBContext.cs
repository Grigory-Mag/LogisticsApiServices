using Microsoft.EntityFrameworkCore;

namespace LogisticsApiServices.DBConverters
{
    public partial class DBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    var mem = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
                    IConfiguration _configuration = mem.Build();
                    var connection = _configuration.GetConnectionString("Postgres");

                    optionsBuilder.UseNpgsql(connection);
                }
            }
        }
    }
}
