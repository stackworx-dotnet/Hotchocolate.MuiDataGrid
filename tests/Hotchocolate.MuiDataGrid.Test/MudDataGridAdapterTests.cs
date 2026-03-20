namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using HotChocolate;
using Stackworx.Hotchocolate.MudDataGrid;

public class MudDataGridAdapterTests
{
    [Fact]
    public void Map_ShouldConvertMudInputToMuiInput()
    {
        var sut = new MudToMuiDataGridAdapter();

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

        var result = sut.Map(input);

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
        var sut = new MudToMuiDataGridAdapter();

        var input = new MudDataGridFilterInput
        {
            SortDefinitions =
            [
                new MudDataGridSortDefinitionInput("first", false),
                new MudDataGridSortDefinitionInput("second", false),
                new MudDataGridSortDefinitionInput("third", true),
            ],
        };

        var result = sut.Map(input);

        result.Sorting!.Select(x => x.Field).Should().Equal("first", "second", "third");
    }

    [Fact]
    public void Map_ShouldFailFastWithGraphqlError_WhenOperatorIsUnsupported()
    {
        var sut = new MudToMuiDataGridAdapter();

        var act = () => sut.Map(
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

