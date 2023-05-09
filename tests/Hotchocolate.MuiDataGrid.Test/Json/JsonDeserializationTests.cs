namespace Stackworx.Hotchocolate.MuiDataGrid.Json;

using System.Text.Json;
using FluentAssertions;

public class JsonDeserializationTests
{
    [Fact]
    public void TestFilter()
    {
        // given
        var jsonString = @"{
    ""items"": [{
        ""field"": ""field1"",
        ""value"": ""f"",
        ""operator"": ""contains""
    }, {
        ""field"": ""field2"",
        ""value"": [""0.5""],
        ""operator"": ""isAnyOf""
    }],
    ""logicOperator"": ""or""
}";

        // when
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var actual = JsonSerializer.Deserialize<MuiDataGridFilterInput>(jsonString, options);

        // then
        actual.Should().BeEquivalentTo(new MuiDataGridFilterInput
        {
            Items = new List<MuiDataGridFilterItemInput>
            {
                new("field1", new MuiValue("f"), "contains"),
                new("field2", new MuiValue(new List<string> { "0.5" }), "isAnyOf"),
            },
            LogicOperator = MuiDataGridLogicOperator.Or,
        });
    }

    [Fact]
    public void TestSort()
    {
        // given
        var jsonString = @"[{
    ""field"": ""field1"",
    ""sort"": ""asc""
}]";

        // when
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var actual = JsonSerializer.Deserialize<IList<MuiDataGridSortItem>>(jsonString, options);

        // then
        actual.Should().Equal(new List<MuiDataGridSortItem>
        {
            new("field1", MuiGridSortDirection.Asc),
        });
    }
}