namespace Stackworx.Hotchocolate.Muidatagrid.GraphQL;

using HotChocolate.Types.Relay;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public sealed class PersonDataType(INodeIdSerializer idSerializer, ExpressionBuilderFlavour flavour = ExpressionBuilderFlavour.EFCORE)
    : DataType<Person>(flavour)
{
    public PersonDataType(ExpressionBuilderFlavour flavour = ExpressionBuilderFlavour.EFCORE)
        : this(new DefaultNodeIdSerializer(), flavour)
    {
    }

    protected override void Configure(DataTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Id).SetNodeIdHandler(idSerializer, "Person");
        builder.Property(p => p.Firstname);
        builder.Property(p => p.Lastname);
        builder.Property(p => p.Bio);
        builder.Property(p => p.Age);
        builder.Property(p => p.Weight);
        builder.Property(p => p.BankAccountBalance);
        builder.Property(p => p.Married);
        builder.Property(p => p.Gender);
        builder.Property(p => p.RefId).SetNodeIdHandler(idSerializer, "Ref");
        builder.Property(p => p.RefIdNullable).SetNodeIdHandler(idSerializer, "Ref");
        builder.Property(p => p.NonGraphQlSerialisedId);
        builder.Property(p => p.DateOfBirth);
        builder.Property(p => p.IdCardReceivedDate);
        builder.Property(p => p.CreatedAtDate);
        builder.Property(p => p.UpdatedAtDate);
        builder.Property(p => p.MarriageDate);
        builder.Property(p => p.Address!.Apartment.Name).SetName("apartmentName");
        builder.Property(p => p.Address!.Apartment.Price);
        builder.Property(p => p.Address!.Apartment.SellDate);
        builder.Property(p => p.Address!.Apartment.ApartmentType)
            .SetName("apartmentType");
    }
}