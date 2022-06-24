namespace Stackworx.Hotchocolate.Muidatagrid.GraphQL;

using System.Linq.Expressions;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public class PersonColumnLookup : BaseColumnLookup<Person>
{
    protected override ColumnLookupMember? InternalLookup(string column)
    {
        switch (column)
        {
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
            case "married":
                return this.GetMemberExpression(p => p.Married);
            case "gender":
                return this.GetMemberExpression(p => p.Gender);
            case "refId":
                return this.GetMemberExpression(p => p.RefId);
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
        }

        return null;
    }
}
