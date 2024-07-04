namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.TestHost;

public static class TestServerExtensions
{
    public static async Task<ClientQueryResult> PostAsync(
        this TestServer testServer,
        ClientQueryRequest request,
        string path = "/graphql")
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await testServer.CreateClient().PostAsync(CreateUrl(path), content);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new ClientQueryResult
            {
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ClientQueryResult>(json);
        ArgumentNullException.ThrowIfNull(result);

        result.StatusCode = response.StatusCode;
        result.ContentType = response.Content.Headers.ContentType!.ToString();
        return result;
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