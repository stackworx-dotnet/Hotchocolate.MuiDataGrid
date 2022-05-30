namespace Stackworx.Hotchocolate.Muidatagrid.Entities;

public class Person
{
    public int Id { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public int Age { get; set; }

    public bool Married { get; set; }

    public Gender Gender { get; set; }

    public double? Weight { get; set; }

    public Guid RefId { get; set; }
}