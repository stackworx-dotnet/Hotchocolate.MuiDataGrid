namespace Stackworx.Hotchocolate.Muidatagrid.GraphQL;

using System.Linq.Expressions;
using Stackworx.Hotchocolate.MuiDataGrid;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public class PersonColumnLookup : BaseColumnLookup<Person>
{
    protected override ColumnLookupMember? InternalLookup(ParameterExpression parameter, string column)
    {
        switch (column)
        {
            case "firstname":
                return this.GetMemberExpression(parameter, p => p.Firstname);
            case "lastname":
                return this.GetMemberExpression(parameter, p => p.Lastname);
            case "bio":
                return this.GetMemberExpression(parameter, p => p.Bio);
            case "age":
                return this.GetMemberExpression(parameter, p => p.Age);
            case "weight":
                return this.GetMemberExpression(parameter, p => p.Weight);
            case "married":
                return this.GetMemberExpression(parameter, p => p.Married);
            case "gender":
                return this.GetMemberExpression(parameter, p => p.Gender);
            case "refId":
                return this.GetMemberExpression(parameter, p => p.RefId);
            case "dateOfBirth":
                return this.GetMemberExpression(parameter, p => p.DateOfBirth);
            case "idCardReceivedDate":
                return this.GetMemberExpression(parameter, p => p.IdCardReceivedDate);
            case "createdAtDate":
                return this.GetMemberExpression(parameter, p => p.CreatedAtDate);
            case "updatedAtDate":
                return this.GetMemberExpression(parameter, p => p.UpdatedAtDate);
            case "marriageDate":
                return this.GetMemberExpression(parameter, p => p.MarriageDate);
        }

        return null;
    }
}