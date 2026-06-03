namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using HotChocolate;
using Stackworx.Hotchocolate.MudDataGrid;

public class MudDataGridAdapterTests
{
    [Fact]
    public void Map_ShouldConvertMudInputToMuiInput()
    {
        var input = new MudDataGridFilterInput
        {
            FilterDefinitions =
            [
                new MudDataGridFilterDefinitionInput("firstname", "contains", new MuiValue("Jo")),
                new MudDataGridFilterDefinitionInput("age", ">=", new MuiValue("18")),
            ],
            SortDefinitions =
            [
                new MudDataGridSortDefinitionInput("lastname", false),
                new MudDataGridSortDefinitionInput("firstname", true),
            ],
        };

        var result = MudToMuiDataGridAdapter.Map(input);

        result.Filters.Should().NotBeNull();
        result.Filters!.LogicOperator.Should().Be(MuiDataGridLogicOperator.And);
        result.Filters.Items.Should().Equal(
            [
                new MuiDataGridFilterItemInput("firstname", new MuiValue("Jo"), "contains", null, "mud"),
                new MuiDataGridFilterItemInput("age", new MuiValue("18"), ">=", null, "mud"),
            ]);

        result.Sorting.Should().Equal(
            [
                new MuiDataGridSortItem("lastname", MuiGridSortDirection.Asc),
                new MuiDataGridSortItem("firstname", MuiGridSortDirection.Desc),
            ]);
    }

    [Fact]
    public void Map_ShouldPreserveSortDefinitionOrder()
    {
        var input = new MudDataGridFilterInput
        {
            SortDefinitions =
            [
                new MudDataGridSortDefinitionInput("first", false),
                new MudDataGridSortDefinitionInput("second", false),
                new MudDataGridSortDefinitionInput("third", true),
            ],
        };

        var result = MudToMuiDataGridAdapter.Map(input);

        result.Sorting!.Select(x => x.Field).Should().Equal("first", "second", "third");
    }

    [Fact]
    public void Map_ShouldMapEnumListWithAnyOf_ToIsAnyOf()
    {
        // Verifies that a MudDataGrid "any of" filter carrying a list of enum string
        // values is forwarded untouched as an isAnyOf MUI filter item.
        var enumValues = new MuiValue(new List<string> { "ADMIN", "SUPER_USER" });

        var input = new MudDataGridFilterInput
        {
            FilterDefinitions =
            [
                new MudDataGridFilterDefinitionInput("roles", "any of", enumValues),
            ],
        };

        var result = MudToMuiDataGridAdapter.Map(input);

        result.Filters.Should().NotBeNull();
        result.Filters!.Items.Should().HaveCount(1);

        var item = result.Filters.Items[0];
        item.Field.Should().Be("roles");
        item.Operator.Should().Be("isAnyOf");
        item.Value.Should().Be(enumValues);
        item.FromInput.Should().Be("mud");
    }

    [Fact]
    public void Map_ShouldMapEnumListWithIsAnyOf_ToIsAnyOf()
    {
        var enumValues = new MuiValue(new List<string> { "READ_ONLY" });

        var input = new MudDataGridFilterInput
        {
            FilterDefinitions =
            [
                new MudDataGridFilterDefinitionInput("roles", "is any of", enumValues),
            ],
        };

        var result = MudToMuiDataGridAdapter.Map(input);

        result.Filters!.Items.Single().Operator.Should().Be("isAnyOf");
        result.Filters.Items.Single().Value.Should().Be(enumValues);
    }

    [Fact]
    public void Map_ShouldPreserveEnumArrayValuesAcrossMapping()
    {
        // Ensures the MuiValue array holding the enum strings is identical by ref
        // so no data is lost during operator normalisation.
        var enumValues = new MuiValue(new List<string> { "ADMIN", "SUPER_USER", "READ_ONLY" });

        var input = new MudDataGridFilterInput
        {
            FilterDefinitions =
            [
                new MudDataGridFilterDefinitionInput("permissions", "any of", enumValues),
            ],
        };

        var result = MudToMuiDataGridAdapter.Map(input);

        var mappedItem = result.Filters!.Items.Single();
        mappedItem.Value.Should().BeSameAs(enumValues, "the original MuiValue array must not be cloned or altered");

        // Verify the string values are accessible and intact
        var arrayValues = mappedItem.Value!.AsArray().Select(v => v.AsString()).ToList();
        arrayValues.Should().Equal("ADMIN", "SUPER_USER", "READ_ONLY");
    }

    [Fact]
    public void Map_ShouldFailFastWithGraphqlError_WhenOperatorIsUnsupported()
    {
        var act = () => MudToMuiDataGridAdapter.Map(
            new MudDataGridFilterInput
            {
                FilterDefinitions =
                [
                    new MudDataGridFilterDefinitionInput("firstname", "not contains", new MuiValue("a")),
                ],
            });

        var exception = act.Should().Throw<GraphQLException>().Which;
        exception.Errors.Should().HaveCount(1);

        var error = exception.Errors[0];
        error.Code.Should().Be("MUD_OPERATOR_NOT_SUPPORTED");
        error.Extensions.Should().ContainKey("field").WhoseValue.Should().Be("firstname");
        error.Extensions.Should().ContainKey("operator").WhoseValue.Should().Be("not contains");
    }
}