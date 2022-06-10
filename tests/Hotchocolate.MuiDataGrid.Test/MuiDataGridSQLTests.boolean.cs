namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestBooleanTrue()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("true"),
                    ColumnField: "married",
                    OperatorValue: "is"),
            },
        };
        var sql = dbContext.People.Where(p => p.Married == true).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestBooleanFalse()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("false"),
                    ColumnField: "married",
                    OperatorValue: "is"),
            },
        };
        var sql = dbContext.People.Where(p => p.Married == false).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}