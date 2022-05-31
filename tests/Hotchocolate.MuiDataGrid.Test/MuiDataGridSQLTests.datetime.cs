namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestDateTimeIsEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"2022-05-31T10:08\""),
                    ColumnField: "dateOfBirth",
                    OperatorValue: "is"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.DateOfBirth == DateTime.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeIsNotEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"2022-05-31T10:08\""),
                    ColumnField: "dateOfBirth",
                    OperatorValue: "not"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.DateOfBirth != DateTime.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeAfter()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"2022-05-31T10:08\""),
                    ColumnField: "dateOfBirth",
                    OperatorValue: "after"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.DateOfBirth > DateTime.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeOnOrAfter()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"2022-05-31T10:08\""),
                    ColumnField: "dateOfBirth",
                    OperatorValue: "onOrAfter"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.DateOfBirth >= DateTime.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeBefore()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"2022-05-31T10:08\""),
                    ColumnField: "dateOfBirth",
                    OperatorValue: "before"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.DateOfBirth < DateTime.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeOnOrBefore()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"2022-05-31T10:08\""),
                    ColumnField: "dateOfBirth",
                    OperatorValue: "onOrBefore"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.DateOfBirth <= DateTime.Parse("2022-05-31T10:08")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeIsEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    ColumnField: "idCardReceivedDate",
                    OperatorValue: "isEmpty"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.IdCardReceivedDate == null).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestDateTimeIsNotEmpty()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: null,
                    ColumnField: "idCardReceivedDate",
                    OperatorValue: "isNotEmpty"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        var sql = dbContext.People.Where(p => p.IdCardReceivedDate != null).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}