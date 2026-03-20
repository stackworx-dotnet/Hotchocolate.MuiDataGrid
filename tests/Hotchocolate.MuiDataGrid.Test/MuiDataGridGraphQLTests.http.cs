namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Snapshooter.Xunit;

public partial class MuiDataGridGraphQLTests
{
    [Fact]
    public async Task TestHttpStaticQuery()
    {
        var server = fixture.CreateTestServer();
        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query people {
                                    people(filters: {
                                        items: [{
                                            field: "firstname",
                                            value: "Celeste",
                                            operator: "equals"
                                        }, {
                                            field: "age",
                                            value: [5, 6],
                                            operator: "isAnyOf"
                                        }]
                                    }) { firstname } 
                                }
                    """,
        };
        var result = await server.PostAsync(request);

        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestHttpVariableQuery()
    {
        var server = fixture.CreateTestServer();

        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query people($filters: MuiDataGridFilterInput!) { 
                                    people(filters: $filters) { firstname } 
                                }
                    """,
            Variables = new Dictionary<string, object>
            {
                {
                    "filters", new Dictionary<string, object>
                    {
                        {
                            "items", new List<object>
                            {
                                new Dictionary<string, object>
                                {
                                    {
                                        "field", "firstname"
                                    },
                                    {
                                        "value", "Celeste"
                                    },
                                    {
                                        "operator", "equals"
                                    },
                                },
                                new Dictionary<string, object>
                                {
                                    {
                                        "field", "age"
                                    },
                                    {
                                        "value", new List<object>
                                        {
                                            "5",
                                            "6",
                                        }
                                    },
                                    {
                                        "operator", "isAnyOf"
                                    },
                                },
                            }
                        },
                    }
                },
            },
        };
        var result = await server.PostAsync(request);

        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestHttpStaticSingleSelectOptionQuery()
    {
        var server = fixture.CreateTestServer();
        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query people { 
                                    people(filters: {
                                        items: [{
                                            field: "nonGraphQlSerialisedId",
                                            value: {label: "Reference1", value: "9F1EF691-2C5C-4BDE-B0BE-635BDD4E180C"},
                                            operator: "equals"
                                        }]
                                    }) { firstname }
                                }
                    """,
        };
        var result = await server.PostAsync(request);
        result.Errors.Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestHttpStaticQueryReturnsDeserializedError()
    {
        var server = fixture.CreateTestServer();
        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query people($filters: MuiDataGridFilterInput!) {
                                    people(filters: $filters) { firstname }
                                }
                    """,
            Variables = new Dictionary<string, object>
            {
                {
                    "filters", new Dictionary<string, object>
                    {
                        {
                            "items", new List<object>
                            {
                                new Dictionary<string, object>
                                {
                                    {
                                        "field", "firstname"
                                    },
                                    {
                                        "value", 123
                                    },
                                    {
                                        "operator", "equals"
                                    },
                                },
                            }
                        },
                    }
                },
            },
        };

        var result = await server.PostAsync(request);

        result.Errors.Should().HaveCount(0);
    }
}
