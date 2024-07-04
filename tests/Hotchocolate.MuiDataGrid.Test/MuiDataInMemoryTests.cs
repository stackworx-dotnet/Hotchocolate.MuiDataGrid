namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Linq.Expressions;
using System.Reflection;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;
using Xunit.Abstractions;

public class MuiDataInMemoryTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void TestInMemory()
    {
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup(), ExpressionBuilderFlavour.IN_MEMORY);

        var people = new List<Person>
        {
            new()
            {
                Bio = null,
            },
            new()
            {
                Bio = "My Bio",
            },
        };
        var filters = builder.Filter(
            new MuiDataGridFilterInput
            {
                Items = new List<MuiDataGridFilterItemInput>
                {
                    new("bio", new MuiValue("Jan"), "contains"),
                },
            });
        outputHelper.WriteLine(filters.ToString());
        var results = people.AsQueryable().Where(filters).ToList();
        // var results = people.AsQueryable().Where(p => ((p.Bio != null) && p.Bio.Contains("Jan"))).ToList();

        // results.Should()
    }

    [Fact]
    public void TestNullable()
    {
        var nullabilityContext = new NullabilityInfoContext();

        Expression<Func<Person, string?>> e1 = p => p.Bio;
        Expression<Func<Person, string>> e2 = p => p.Firstname;

        MemberExpression t1 = (MemberExpression)e1.Body;
        MemberExpression t2 = (MemberExpression)e2.Body;
        var ctx1 = nullabilityContext.Create((PropertyInfo)t1.Member);
        var ctx2 = nullabilityContext.Create((PropertyInfo)t2.Member);
        outputHelper.WriteLine(ctx1.ToString());
    }
}