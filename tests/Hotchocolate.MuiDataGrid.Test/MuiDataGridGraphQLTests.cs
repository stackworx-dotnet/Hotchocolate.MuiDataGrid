namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using HotChocolate.Execution;
using Snapshooter.Xunit;
using Stackworx.Hotchocolate.MuiDataGrid;

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
    public async Task TestExecute()
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
                                Value: new MuiValue("\"Ciaran\""),
                                OperatorValue: "equals"),
                        },
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }
}