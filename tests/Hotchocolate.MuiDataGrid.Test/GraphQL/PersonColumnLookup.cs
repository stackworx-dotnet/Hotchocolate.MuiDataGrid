namespace Stackworx.Hotchocolate.Muidatagrid.GraphQL;

using Stackworx.Hotchocolate.Muidatagrid.Entities;

public class PersonColumnLookup : BaseColumnLookup<Person>
{
    protected override ColumnLookupMember? InternalLookup(string column)
    {
        switch (column)
        {
            case "id":
                return this.GetMemberExpression(p => p.Id);
            case "firstname":
                return this.GetMemberExpression(p => p.Firstname);
            case "lastname":
                return this.GetMemberExpression(p => p.Lastname);
            case "bio":
                return this.GetMemberExpression(p => p.Bio);
            case "age":
                return this.GetMemberExpression(p => p.Age);
            case "weight":
                return this.GetMemberExpression(p => p.Weight);
            case "bankAccountBalance":
                return this.GetMemberExpression(p => p.BankAccountBalance);
            case "married":
                return this.GetMemberExpression(p => p.Married);
            case "gender":
                return this.GetMemberExpression(p => p.Gender);
            case "refId":
                return this.GetMemberExpression(p => p.RefId);
            case "refIdNullable":
                return this.GetMemberExpression(p => p.RefIdNullable);
            case "nonGraphQlSerialisedId":
                return this.GetMemberExpression(p => p.NonGraphQlSerialisedId);
            case "dateOfBirth":
                return this.GetMemberExpression(p => p.DateOfBirth);
            case "idCardReceivedDate":
                return this.GetMemberExpression(p => p.IdCardReceivedDate);
            case "createdAtDate":
                return this.GetMemberExpression(p => p.CreatedAtDate);
            case "updatedAtDate":
                return this.GetMemberExpression(p => p.UpdatedAtDate);
            case "marriageDate":
                return this.GetMemberExpression(p => p.MarriageDate);
            case "apartmentName":
                return this.GetMemberExpression(p => p.Address!.Apartment.Name);
            case "price":
                return this.GetMemberExpression(p => p.Address!.Apartment.Price);
            case "sellDate":
                return this.GetMemberExpression(p => p.Address!.Apartment.SellDate);
            case "apartmentType":
                return this.GetMemberExpression(p => p.Address!.Apartment.ApartmentType);
        }

        return null;
    }
}
