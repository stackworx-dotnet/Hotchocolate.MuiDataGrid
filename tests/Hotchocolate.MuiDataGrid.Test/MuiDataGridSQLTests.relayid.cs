namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using HotChocolate;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestRefIdIsEqual()
    {
        var idSerializer = this.fixture.Services.GetRequiredService<IIdSerializer>();
        var g = Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B");
        var relayId = idSerializer.Serialize(Schema.DefaultName, "Ref", g);
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue($"\"{relayId}\""),
                    ColumnField: "refId",
                    OperatorValue: "is"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("refId", new DefaultRelayIdSingleSelectHandler<Person>(idSerializer, "Ref"));

        var sql = dbContext.People.Where(p => p.RefId == Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    [Fact]
    public async Task TestRefIdIsNotEqual()
    {
        var idSerializer = this.fixture.Services.GetRequiredService<IIdSerializer>();
        var g = Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B");
        var relayId = idSerializer.Serialize(Schema.DefaultName, "Ref", g);
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue($"\"{relayId}\""),
                    ColumnField: "refId",
                    OperatorValue: "not"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("refId", new DefaultRelayIdSingleSelectHandler<Person>(idSerializer, "Ref"));

        var sql = dbContext.People.Where(p => p.RefId != Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B")).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }

    /*
    [Fact]
    public async Task TestRefIdIsAnyOf()
    {
        var idSerializer = this.fixture.Services.GetRequiredService<IIdSerializer>();
        var g = Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B");
        var relayId = idSerializer.Serialize(Schema.DefaultName, "Ref", g);
        await using var dbContext = await this.fixture.CreateDbContextAsync();

        var filters = new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new(
                    Value: new MuiValue($"[\"{relayId}\"]"),
                    ColumnField: "refId",
                    OperatorValue: "isAnyOf"),
            },
        };
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("refId", new DefaultRelayIdSingleSelectHandler<Person>(idSerializer, "Ref"));

        var sql = dbContext.People.Where(p => new List<Guid> { Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180B") }.Contains(p.RefId)).ToQueryString();
        var muiSql = dbContext.People.Where(builder.Filter(filters)).ToQueryString();
        muiSql.Should().Be(sql);
        muiSql.MatchSnapshot();
    }
    */
}