namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.MuiDataGrid;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

[Collection(nameof(DbFixtureCollection))]
public partial class MuiDataGridGraphQLTests
{
    private readonly DbFixture fixture;

    public MuiDataGridGraphQLTests(DbFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void TestSchema()
    {
        this.fixture.RequestExecutor.Schema.Print().MatchSnapshot();
    }

    [Fact]
    public async Task TestOrLinkOperatorExecute()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(new MuiDataGridFilterInput
                    {
                        Items = new List<MuiDataGridFilterItemInput>
                        {
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("\"Celeste\""),
                                OperatorValue: "equals"),
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("\"Johanny\""),
                                OperatorValue: "equals"),
                        },
                        LinkOperator = MuiDataGridLinkOperator.Or,
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestAndLinkOperatorExecute()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(new MuiDataGridFilterInput
                    {
                        Items = new List<MuiDataGridFilterItemInput>
                        {
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("\"Celeste\""),
                                OperatorValue: "equals"),
                            new(
                                ColumnField: "lastname",
                                Value: new MuiValue("\"Le Roux\""),
                                OperatorValue: "equals"),
                        },
                        LinkOperator = MuiDataGridLinkOperator.And,
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestChainFiltersTogetherExecute()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname,gender, dateOfBirth } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(new MuiDataGridFilterInput
                    {
                        Items = new List<MuiDataGridFilterItemInput>
                        {
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("\"Celeste\""),
                                OperatorValue: "equals"),
                            new(
                                ColumnField: "lastname",
                                Value: new MuiValue("\"Le Roux\""),
                                OperatorValue: "equals"),
                            // new(
                            //     ColumnField: "gender",
                            //     Value: new MuiValue("\"MALE\""),
                            //     OperatorValue: "is"),
                            // TODO: this breaks on linux
                            /*
                            new(
                                ColumnField: "dateOfBirth",
                                Value: new MuiValue("\"2022-05-27T13:41\""),
                                OperatorValue: "after"),
                            */
                            new(
                                ColumnField: "marriageDate",
                                Value: null,
                                OperatorValue: "isNotEmpty"),
                        },
                        LinkOperator = MuiDataGridLinkOperator.And,
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestPersonQuery()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people { people { firstname } }");
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    // [Fact]
    // public async Task TestSingleSelectEnumExecute()
    // {
    //     var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
    //     builder.AddHandler("gender", new PersonGenderHandler());
    //     var result = await this.fixture.RequestExecutor.ExecuteAsync(
    //         "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname } }",
    //         new Dictionary<string, object?>
    //         {
    //             {
    //                 "filters", this.ConvertFilterInput(new MuiDataGridFilterInput
    //                 {
    //                     Items = new List<MuiDataGridFilterItemInput>
    //                     {
    //                         new(
    //                             ColumnField: "gender",
    //                             Value: new MuiValue("\"MALE\""),
    //                             OperatorValue: "is"),
    //                     },
    //                 })
    //             },
    //         });
    //     result.Errors.Should().BeNull();
    //     result.MatchSnapshot();
    // }
    private class PersonGenderHandler : DefaultEnumSingleSelectHandler<Person, Gender>
    {
    }
}