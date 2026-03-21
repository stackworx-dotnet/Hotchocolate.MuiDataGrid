namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestDateOnlyIsEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("2022-05-31"),
                    Field: "createdAtDate",
                    Operator: "is"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.CreatedAtDate == DateOnly.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyIsNotEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("2022-05-31"),
                    Field: "createdAtDate",
                    Operator: "not"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.CreatedAtDate != DateOnly.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyAfter()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("2022-05-31"),
                    Field: "createdAtDate",
                    Operator: "after"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.CreatedAtDate > DateOnly.Parse("2022-05-31")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyOnOrAfter()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("2022-05-31"),
                    Field: "createdAtDate",
                    Operator: "onOrAfter"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.CreatedAtDate >= DateOnly.Parse("2022-05-31")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyBefore()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("2022-05-31"),
                    Field: "createdAtDate",
                    Operator: "before"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.CreatedAtDate < DateOnly.Parse("2022-05-31")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyOnOrBefore()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("2022-05-31"),
                    Field: "createdAtDate",
                    Operator: "onOrBefore"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.CreatedAtDate <= DateOnly.Parse("2022-05-31")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyIsEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    Field: "updatedAtDate",
                    Operator: "isEmpty"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.UpdatedAtDate == null).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateOnlyIsNotEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    Field: "updatedAtDate",
                    Operator: "isNotEmpty"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => p.UpdatedAtDate != null).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}