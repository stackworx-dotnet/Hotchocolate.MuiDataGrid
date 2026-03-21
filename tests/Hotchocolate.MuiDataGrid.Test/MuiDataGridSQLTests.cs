namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;
using Xunit.Abstractions;

[Collection(nameof(DbFixtureCollection))]
public partial class MuiDataGridSQLTests
{
    private readonly DbFixture fixture;
    private readonly ITestOutputHelper outputHelper;

    public MuiDataGridSQLTests(DbFixture fixture, ITestOutputHelper outputHelper)
    {
        this.fixture = fixture;
        this.outputHelper = outputHelper;
    }

    [Fact]
    public async Task TestEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new PersonDataType();
        var sql = dbContext.People.ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(new MuiDataGridFilterInput())).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringEquals()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var builder = new PersonDataType();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("Ciaran"),
                    Field: "firstname",
                    Operator: "equals"),
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
                    Value: new MuiValue("Ciaran"),
                    Field: "firstname",
                    Operator: "contains"),
            },
        };
        var builder = new PersonDataType();
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
                    Value: new MuiValue("Ciaran"),
                    Field: "firstname",
                    Operator: "startsWith"),
            },
        };
        var builder = new PersonDataType();
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
                    Value: new MuiValue("Ciaran"),
                    Field: "firstname",
                    Operator: "endsWith"),
            },
        };
        var builder = new PersonDataType();
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
                    Field: "bio",
                    Operator: "isEmpty"),
            },
        };
        var builder = new PersonDataType();
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
                    Field: "bio",
                    Operator: "isNotEmpty"),
            },
        };
        var builder = new PersonDataType();
        var sql = dbContext.People.Where(p => !string.IsNullOrEmpty(p.Bio)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestStringIsAnyOf()
    {
        var values = new List<string>
        {
            "John",
            "Harry",
        };
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue(values),
                    Field: "firstname",
                    Operator: "isAnyOf"),
            },
        };
        var builder = new PersonDataType();
        // TODO: figure out why this resolves to different sql than the same contains value. json_each has something to do with it
        // https://github.com/jOOQ/jOOQ/issues/11349
        var sql = dbContext.People.Where(p => values.Contains(p.Firstname)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should()
            .Contain(
                """
                SELECT "p"."Id", "p"."AddressId", "p"."Age", "p"."BankAccountBalance", "p"."Bio", "p"."CreatedAtDate", "p"."DateOfBirth", "p"."Firstname", "p"."Gender", "p"."IdCardReceivedDate", "p"."Lastname", "p"."MarriageDate", "p"."Married", "p"."NonGraphQlSerialisedId", "p"."RefId", "p"."RefIdNullable", "p"."UpdatedAtDate", "p"."Weight"
                FROM "People" AS "p"
                WHERE "p"."Firstname" IN (
                """);
        muiSql.MatchSnapshot();
    }
}