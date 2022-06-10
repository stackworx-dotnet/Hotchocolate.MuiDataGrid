namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using HotChocolate.Execution;

public partial class MuiDataGridSQLTests
{
    [Fact]
    public async Task TestStringMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: ""s"", columnField: ""column"", operatorValue: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.Errors.Should().BeNull();
        result.ExpectQueryResult().Data!["input"]!.ToString().Should().Be(@"s");
    }

    [Fact]
    public async Task TestNumberMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: 5, columnField: ""column"", operatorValue: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.Errors.Should().BeNull();
        result.ExpectQueryResult().Data!["input"]!.ToString().Should().Be(@"5");
    }

    [Fact]
    public async Task TestNullMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {columnField: ""column"", operatorValue: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.Errors.Should().BeNull();
        string? input = result.ExpectQueryResult().Data!["input"]?.ToString();
        input.Should().Be(null);
    }

    [Fact]
    public async Task TestDateMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: ""2022-06-24"" columnField: ""column"", operatorValue: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.Errors.Should().BeNull();
        var input = result.ExpectQueryResult().Data!["input"]?.ToString();
        input.Should().Be("2022-06-24");
    }

    /*
    [Fact]
    public async Task TestStringListMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: [""5"", ""6""] columnField: ""column"", operatorValue: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.Errors.Should().BeNull();
        var input = result.ExpectQueryResult().Data!["input"]?.ToString();
        input.Should().Be("\"2022-06-24\"");
    }
    */
}