namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestGenderIsEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"MALE\""),
                    ColumnField: "gender",
                    OperatorValue: "is"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("gender", new DefaultEnumSingleSelectHandler<Person, Gender>());

        var sql = dbContext.People.Where(p => p.Gender == Gender.MALE).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestGenderIsNotEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("\"MALE\""),
                    ColumnField: "gender",
                    OperatorValue: "not"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("gender", new PersonGenderSingleSelectHandler());

        var sql = dbContext.People.Where(p => p.Gender != Gender.MALE).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestGenderIsAnyOf()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("[\"MALE\", \"FEMALE\"]"),
                    ColumnField: "gender",
                    OperatorValue: "isAnyOf"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("gender", new PersonGenderSingleSelectHandler());

        var sql = dbContext.People.Where(p => new List<Gender> { Gender.MALE, Gender.FEMALE }.Contains(p.Gender)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
}