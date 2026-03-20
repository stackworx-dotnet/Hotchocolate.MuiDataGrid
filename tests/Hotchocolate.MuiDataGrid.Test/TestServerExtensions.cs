namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.TestHost;

public static class TestServerExtensions
{
    private static readonly Regex StackTraceUnixPathRegex = new(
        @" in /.*?\.cs:line (?<line>\d+)",
        RegexOptions.Compiled);

    private static readonly Regex StackTraceWindowsPathRegex = new(
        @" in [A-Za-z]:\\.*?\.cs:line (?<line>\d+)",
        RegexOptions.Compiled);

    private static readonly Regex LambdaMethodRegex = new(
        @"lambda_method\d+",
        RegexOptions.Compiled);

    public static async Task<ClientQueryResult> PostAsync(
        this TestServer testServer,
        ClientQueryRequest request,
        string path = "/graphql")
    {
        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        var response = await testServer.CreateClient().PostAsync(CreateUrl(path), content);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new ClientQueryResult
            {
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ClientQueryResult>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        ArgumentNullException.ThrowIfNull(result);

        result.StatusCode = response.StatusCode;
        result.ContentType = response.Content.Headers.ContentType!.ToString();
        SanitizeMachinePaths(result);
        return result;
    }

    private static void SanitizeMachinePaths(ClientQueryResult result)
    {
        foreach (var error in result.Errors)
        {
            if (!error.Extensions.TryGetValue("stackTrace", out var stackTrace) || string.IsNullOrWhiteSpace(stackTrace))
            {
                continue;
            }

            var sanitized = StackTraceUnixPathRegex.Replace(stackTrace, " in <source>:line ${line}");
            sanitized = StackTraceWindowsPathRegex.Replace(sanitized, " in <source>:line ${line}");
            sanitized = LambdaMethodRegex.Replace(sanitized, "lambda_methodX");
            error.Extensions["stackTrace"] = sanitized;
        }
    }

    public static string CreateUrl(string? path)
    {
        var url = "http://localhost:5000";

        if (path is not null)
        {
            url += "/" + path.TrimStart('/');
        }

        return url;
    }
}