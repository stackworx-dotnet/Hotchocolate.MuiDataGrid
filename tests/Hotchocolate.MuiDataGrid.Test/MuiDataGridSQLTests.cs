namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

[Collection(nameof(DbFixtureCollection))]
public partial class MuiDataGridSQLTests
{
    private readonly DbFixture fixture;

    public MuiDataGridSQLTests(DbFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task TestEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(new MuiDataGridFilterInput())).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringEquals()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"Ciaran\""),
                    ColumnField: "firstname",
                    OperatorValue: "equals"),
            },
        };
        var sql = dbContext.People.Where(p => p.Firstname.Equals("Ciaran")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringContains()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"Ciaran\""),
                    ColumnField: "firstname",
                    OperatorValue: "contains"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Firstname.Contains("Ciaran")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringStartsWith()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"Ciaran\""),
                    ColumnField: "firstname",
                    OperatorValue: "startsWith"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Firstname.StartsWith("Ciaran")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringEndsWith()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"Ciaran\""),
                    ColumnField: "firstname",
                    OperatorValue: "endsWith"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Firstname.EndsWith("Ciaran")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringIsEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    ColumnField: "bio",
                    OperatorValue: "isEmpty"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => string.IsNullOrEmpty(p.Bio)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringIsNotEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    ColumnField: "bio",
                    OperatorValue: "isNotEmpty"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => !string.IsNullOrEmpty(p.Bio)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringIsAnyOf()
    {
        var values = new List<string> { "John", "Harry" };
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("[\"John\", \"Harry\"]"),
                    ColumnField: "firstname",
                    OperatorValue: "isAnyOf"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => values.Contains(p.Firstname)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}