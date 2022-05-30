namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

using Microsoft.EntityFrameworkCore;

public class MuiDataGridDbContext : DbContext
{
    public MuiDataGridDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Person> People => this.Set<Person>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(Console.WriteLine);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties(typeof(Enum))
            .HaveConversion<string>();
    }
}