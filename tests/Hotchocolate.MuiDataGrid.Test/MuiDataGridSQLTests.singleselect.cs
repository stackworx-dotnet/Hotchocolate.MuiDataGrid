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
                    Value: new MuiValue("MALE"),
                    Field: "gender",
                    Operator: "is"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("gender", new PersonGenderHandler());

        var sql = dbContext.People.Where(p => p.Gender == Gender.Male).ToQueryString();
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
                    Value: new MuiValue("MALE"),
                    Field: "gender",
                    Operator: "not"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("gender", new PersonGenderHandler());

        var sql = dbContext.People.Where(p => p.Gender != Gender.Male).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestApartmentTypeIsEqual()
    {
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue("RENTAL_PROPERTY"),
                    Field: "apartmentType",
                    Operator: "is"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("apartmentType", new PersonApartmentTypeHandler());

        var sql = dbContext.People
            .Where(p => p.Address!.Apartment.ApartmentType == ApartmentType.RentalProperty)
            .ToQueryString();
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
                    Value: new MuiValue(new List<string>
                    {
                        "MALE",
                        "FEMALE",
                    }),
                    Field: "gender",
                    Operator: "isAnyOf"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("gender", new PersonGenderHandler());

        var sql = dbContext.People.Where(p => new List<Gender>
        {
            Gender.Male,
            Gender.Female,
        }.Contains(p.Gender)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    private class PersonGenderHandler : DefaultEnumSingleSelectHandler<Person, Gender>
    {
    }

    private class PersonApartmentTypeHandler : DefaultEnumSingleSelectHandler<Person, ApartmentType>
    {
    }
}