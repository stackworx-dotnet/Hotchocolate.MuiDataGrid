namespace Stackworx.Hotchocolate.MuiDataGrid;

using FluentAssertions;
using Snapshooter.Xunit;

public partial class MuiDataGridGraphQLTests
{
    [Fact]
    public async Task TestMudHttpStaticQuery()
    {
        var server = fixture.CreateTestServer();
        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query mudPeople {
                                    mudPeople(filters: {
                                        filterDefinitions: [{
                                            field: "firstname",
                                            value: "Celeste",
                                            operator: "equal"
                                        }, {
                                            field: "age",
                                            value: [5, 6],
                                            operator: "any of"
                                        }],
                                        sortDefinitions: []
                                    }) { firstname }
                                }
                    """,
        };
        var result = await server.PostAsync(request);

        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestMudHttpVariableQuery()
    {
        var server = fixture.CreateTestServer();

        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query mudPeople($filters: MudDataGridFilterInput!) {
                                    mudPeople(filters: $filters) { firstname }
                                }
                    """,
            Variables = new Dictionary<string, object>
            {
                {
                    "filters", new Dictionary<string, object>
                    {
                        {
                            "filterDefinitions", new List<object>
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
                                        "operator", "equal"
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
                                            5,
                                            6,
                                        }
                                    },
                                    {
                                        "operator", "any of"
                                    },
                                },
                            }
                        },
                        {
                            "sortDefinitions", new List<object>()
                        },
                    }
                },
            },
        };
        var result = await server.PostAsync(request);

        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestMudHttpStaticSingleSelectOptionQuery()
    {
        var server = fixture.CreateTestServer();
        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query mudPeople {
                                    mudPeople(filters: {
                                        filterDefinitions: [{
                                            field: "nonGraphQlSerialisedId",
                                            value: {label: "Reference1", value: "9F1EF691-2C5C-4BDE-B0BE-635BDD4E180C"},
                                            operator: "equal"
                                        }],
                                        sortDefinitions: []
                                    }) { firstname }
                                }
                    """,
        };
        var result = await server.PostAsync(request);
        result.Errors.Should().BeEmpty();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestMudHttpStaticQueryReturnsDeserializedError()
    {
        var server = fixture.CreateTestServer();
        var request = new ClientQueryRequest
        {
            Id = "1",
            Query = """
                    query mudPeople($filters: MudDataGridFilterInput!) {
                                    mudPeople(filters: $filters) { firstname }
                                }
                    """,
            Variables = new Dictionary<string, object>
            {
                {
                    "filters", new Dictionary<string, object>
                    {
                        {
                            "filterDefinitions", new List<object>
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
                                        "operator", "equal"
                                    },
                                },
                            }
                        },
                        {
                            "sortDefinitions", new List<object>()
                        },
                    }
                },
            },
        };

        var result = await server.PostAsync(request);

        result.Errors.Should().HaveCount(0);
    }
}
