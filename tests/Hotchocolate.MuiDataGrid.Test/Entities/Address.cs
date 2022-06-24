namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
    public int Id { get; set; }

    public int StreetNumber { get; set; }

    public string Province { get; set; } = string.Empty;

    public string StreetLine1 { get; set; } = string.Empty;

    public string StreetLine2 { get; set; } = string.Empty;

    public int ApartmentId { get; set; }

    [ForeignKey(nameof(ApartmentId))]
    public Apartment Apartment { get; set; } = null!;
}
