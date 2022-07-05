namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

public class Apartment
{
    public int Id { get; set; }

    public int HouseNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime? SellDate { get; set; }
}
