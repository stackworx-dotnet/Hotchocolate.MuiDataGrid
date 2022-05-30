namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestNumberIsEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"5\""),
                    ColumnField: "age",
                    OperatorValue: "="),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Age == 5).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberIsNotEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"5\""),
                    ColumnField: "age",
                    OperatorValue: "!="),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Age != 5).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberGreaterThan()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"5\""),
                    ColumnField: "age",
                    OperatorValue: ">"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Age > 5).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberGreaterThanOrEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"5\""),
                    ColumnField: "age",
                    OperatorValue: ">="),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Age >= 5).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberLessThan()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"5\""),
                    ColumnField: "age",
                    OperatorValue: "<"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Age < 5).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberLessThanOrEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"5\""),
                    ColumnField: "age",
                    OperatorValue: "<="),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Age <= 5).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberIsEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    ColumnField: "weight",
                    OperatorValue: "isEmpty"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Weight == null).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberIsNotEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    ColumnField: "weight",
                    OperatorValue: "isNotEmpty"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.Weight != null).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestNumberIsAnyOf()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();
        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("[\"5\", \"6\"]"),
                    ColumnField: "age",
                    OperatorValue: "isAnyOf"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => new List<int> { 5, 6 }.Contains(p.Age)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}