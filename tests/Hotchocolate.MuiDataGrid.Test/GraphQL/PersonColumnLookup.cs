namespace Stackworx.Hotchocolate.Muidatagrid.GraphQL;

using System.Linq.Expressions;
using Stackworx.Hotchocolate.MuiDataGrid;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public class PersonColumnLookup : BaseColumnLookup<Person>
{
    public override bool CanHandle(string column)
    {
        switch (column)
        {
            case "firstname":
                return true;
            case "bio":
                return true;
            case "age":
                return true;
            case "weight":
                return true;
            case "gender":
                return true;
            case "refId":
                return true;
        }

        return false;
    }

    protected override ColumnLookupMember? InternalLookup(ParameterExpression parameter, string column)
    {
        switch (column)
        {
            case "firstname":
                return this.GetMemberExpression(parameter, p => p.Firstname);
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
        }

        return null;
    }
}