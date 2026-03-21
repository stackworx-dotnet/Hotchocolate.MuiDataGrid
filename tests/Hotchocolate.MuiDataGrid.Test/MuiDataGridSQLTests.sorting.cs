namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestSingleSorting()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new PersonDataType();
        var sorting = new List<MuiDataGridSortItem>
        {
            new("firstname", MuiGridSortDirection.Asc),
        };
        var sql = dbContext.People.OrderBy(p => p.Firstname).ToQueryString();
        var muiSql = builder.Sort(dbContext.People, sorting).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestMultiSorting()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new PersonDataType();
        var sorting = new List<MuiDataGridSortItem>
        {
            new("firstname", MuiGridSortDirection.Asc),
            new("weight", MuiGridSortDirection.Desc),
        };
        var sql = dbContext.People
            .OrderBy(p => p.Firstname)
            .OrderByDescending(p => p.Weight).ToQueryString();
        var muiSql = builder.Sort(dbContext.People, sorting).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}