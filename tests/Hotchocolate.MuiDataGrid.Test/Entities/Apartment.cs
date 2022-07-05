namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

using System.Numerics;

public class Apartment
{
    public int Id { get; set; }

    public int HouseNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
