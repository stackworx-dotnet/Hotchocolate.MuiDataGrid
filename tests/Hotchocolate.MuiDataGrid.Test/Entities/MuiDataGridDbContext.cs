namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

using Microsoft.EntityFrameworkCore;

public class MuiDataGridDbContext : DbContext
{
    public MuiDataGridDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Person> People => this.Set<Person>();

    public DbSet<Address> Addresses => this.Set<Address>();

    public DbSet<Apartment> Apartments => this.Set<Apartment>();

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties(typeof(Enum))
            .HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var g1 = Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B");
        var g2 = Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180C");

        var apartment1 = new Apartment()
        {
            Id = 1,
            Name = "House 1",
            HouseNumber = 56,
            Price = 13.89m,
        };

        var apartment2 = new Apartment()
        {
            Id = 2,
            Name = "House Number 2",
            HouseNumber = 4,
            Price = 15.78m,
            SellDate = DateTime.Now,
        };

        builder.Entity<Apartment>().ToTable("Apartment").HasData(
            apartment1,
            apartment2);

        var address1 = new Address()
        {
            Id = 1,
            Province = "Limpopo",
            StreetLine1 = "Wroxham Road",
            ApartmentId = 1,
        };

        var address2 = new Address()
        {
            Id = 2,
            Province = "Gauteng",
            ApartmentId = 2,
        };

        builder.Entity<Address>().ToTable("Address").HasData(
            address1,
            address2);

        builder.Entity<Person>().ToTable("People").HasData(
            new Person()
            {
                Id = 1,
                Firstname = "Johanny",
                Lastname = "Klein",
                Age = 18,
                Bio = "I like water",
                Gender = Gender.MALE,
                Married = false,
                Weight = 100,
                RefId = g1,
                CreatedAtDate = DateOnly.Parse("2022-05-31"),
                DateOfBirth = DateTime.Now,
                UpdatedAtDate = DateOnly.Parse("2022-05-31"),
                IdCardReceivedDate = DateTime.Now,
                AddressId = 1,
            },
            new Person()
            {
                Id = 2,
                Firstname = "Stacey",
                Lastname = "Pop",
                Age = 36,
                Bio = "I like Chocolate",
                Gender = Gender.FEMALE,
                Married = true,
                Weight = 76,
                RefId = g1,
                CreatedAtDate = DateOnly.Parse("2022-05-30"),
                DateOfBirth = DateTime.Now.AddDays(-1),
                UpdatedAtDate = DateOnly.Parse("2022-05-30"),
                IdCardReceivedDate = DateTime.Now.AddDays(-1),
                AddressId = 2,
            },
            new Person()
            {
                Id = 3,
                Firstname = "Susie",
                Lastname = "van zyl",
                Age = 11,
                Bio = "I like toys",
                Gender = Gender.FEMALE,
                Married = false,
                Weight = 30,
                RefId = g1,
                CreatedAtDate = DateOnly.Parse("2022-05-29"),
                DateOfBirth = DateTime.Now.AddDays(-2),
                UpdatedAtDate = DateOnly.Parse("2022-05-29"),
                IdCardReceivedDate = DateTime.Now.AddDays(-2),
                AddressId = 2,
            },
            new Person()
            {
                Id = 4,
                Firstname = "Johan",
                Lastname = "Groot",
                Age = 55,
                Bio = "I like hunting",
                Gender = Gender.MALE,
                Married = true,
                Weight = 112,
                RefId = g1,
                CreatedAtDate = DateOnly.Parse("2022-05-28"),
                DateOfBirth = DateTime.Now.AddDays(-3),
                UpdatedAtDate = DateOnly.Parse("2022-05-28"),
                IdCardReceivedDate = DateTime.Now.AddDays(-3),
                AddressId = 1,
            },
            new Person()
            {
                Id = 5,
                Firstname = "Celeste",
                Lastname = "Le Roux",
                Age = 26,
                Bio = "I like art",
                Gender = Gender.FEMALE,
                Married = false,
                Weight = 74,
                RefId = g2,
                CreatedAtDate = DateOnly.Parse("2022-05-27"),
                DateOfBirth = DateTime.Now.AddDays(-4),
                UpdatedAtDate = DateOnly.Parse("2022-05-27"),
                IdCardReceivedDate = DateTime.Now.AddDays(-4),
                AddressId = 2,
            });
    }
}