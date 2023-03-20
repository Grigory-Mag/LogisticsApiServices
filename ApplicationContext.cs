using GrpcGreeter.DBPostModels;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    /*
    public DbSet<NewTestTable> NewTestTable { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewTestTable>().HasData(
                new NewTestTable { Id = 1, StringData = "Tom", IntData = 37 },
                new NewTestTable { Id = 2, StringData = "Bob", IntData = 41 },
                new NewTestTable { Id = 3, StringData = "Sam", IntData = 24 }
        );
    }
    */

}