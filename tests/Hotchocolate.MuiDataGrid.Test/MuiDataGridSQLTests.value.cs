namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Text.Json;
using FluentAssertions;
using HotChocolate;
using HotChocolate.Execution;

public partial class MuiDataGridSQLTests
{
    // HC16: the result data model changed (OperationResultData is not indexable). Read the
    // "input" field back from the JSON-serialised result instead.
    private static string? InputValue(IExecutionResult result)
    {
        using var document = JsonDocument.Parse(result.ToJson());
        var input = document.RootElement.GetProperty("data").GetProperty("input");
        return input.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String => input.GetString(),
            _ => input.GetRawText(),
        };
    }

    [Fact]
    public async Task TestStringMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: ""s"", field: ""column"", operator: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        InputValue(result).Should().Be("s");
    }

    [Fact]
    public async Task TestNumberMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: 5, field: ""column"", operator: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        InputValue(result).Should().Be("5");
    }

    [Fact]
    public async Task TestNullMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {field: ""column"", operator: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        var input = InputValue(result);
        input.Should().Be(null);
    }

    [Fact]
    public async Task TestDateMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: ""2022-06-24"" field: ""column"", operator: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        var input = InputValue(result);
        input.Should().Be("2022-06-24");
    }

    [Fact]
    public async Task TestSingleSelectOptionMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: {label: ""Person"", value: ""UHJvZHVjdAppMQ==""}, field: ""column"", operator: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        var input = InputValue(result);
        input.Should().Be("UHJvZHVjdAppMQ==");
    }

    /*
    [Fact]
    public async Task TestStringListMuiValue()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            @"query { input(input: {value: [""5"", ""6""] columnField: ""column"", operatorValue: ""equals""} ) }",
            new Dictionary<string, object?>());
        result.Errors.Should().BeNull();
        var input = InputValue(result);
        input.Should().Be("\"2022-06-24\"");
    }
    */
}