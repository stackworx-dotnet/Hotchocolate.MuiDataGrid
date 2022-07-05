namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Collections.Immutable;
using FluentAssertions;
using HotChocolate.Execution;
using Snapshooter.Xunit;

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
    public async Task TestOrLinkOperatorExecute()
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
                                Value: new MuiValue("\"Celeste\""),
                                OperatorValue: "equals"),
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("\"Johanny\""),
                                OperatorValue: "equals"),
                        },
                        LinkOperator = MuiDataGridLinkOperator.Or,
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestAndLinkOperatorExecute()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(new MuiDataGridFilterInput
                    {
                        Items = new List<MuiDataGridFilterItemInput>
                        {
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("Celeste"),
                                OperatorValue: "equals"),
                            new(
                                ColumnField: "lastname",
                                Value: new MuiValue("Le Roux"),
                                OperatorValue: "equals"),
                        },
                        LinkOperator = MuiDataGridLinkOperator.And,
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestChainFiltersTogetherExecute()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname,gender, dateOfBirth } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(new MuiDataGridFilterInput
                    {
                        Items = new List<MuiDataGridFilterItemInput>
                        {
                            new(
                                ColumnField: "firstname",
                                Value: new MuiValue("Celeste"),
                                OperatorValue: "equals"),
                            new(
                                ColumnField: "lastname",
                                Value: new MuiValue("Le Roux"),
                                OperatorValue: "equals"),
                            // new(
                            //     ColumnField: "gender",
                            //     Value: new MuiValue("MALE"),
                            //     OperatorValue: "is"),
                            // TODO: this breaks on linux
                            /*
                            new(
                                ColumnField: "dateOfBirth",
                                Value: new MuiValue("2022-05-27T13:41"),
                                OperatorValue: "after"),
                            */
                            new(
                                ColumnField: "marriageDate",
                                Value: null,
                                OperatorValue: "isNotEmpty"),
                        },
                        LinkOperator = MuiDataGridLinkOperator.And,
                    })
                },
            });
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestPersonQuery()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people { people { firstname } }");
        result.Errors.Should().BeNull();
        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestPersonAddressNestedQuery()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname address { apartment { houseNumber name } } } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", new Dictionary<string, object?>
                    {
                        {
                            "items", new List<Dictionary<string, object>>
                            {
                                new()
                                {
                                    {
                                        "columnField", "apartmentName"
                                    },
                                    {
                                        "value", new MuiValue("Number")
                                    },
                                    {
                                        "operatorValue", "contains"
                                    },
                                    {
                                        "id", 1342532
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.Errors.Should().BeNull();
    }

    [Fact]
    public async Task TestDecimalQuery()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname address { apartment { houseNumber price } } } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", new Dictionary<string, object?>
                    {
                        {
                            "items", new List<Dictionary<string, object>>
                            {
                                new()
                                {
                                    {
                                        "columnField", "price"
                                    },
                                    {
                                        "value", new MuiValue("14")
                                    },
                                    {
                                        "operatorValue", ">="
                                    },
                                    {
                                        "id", 1342531
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.Errors.Should().BeNull();
    }

    [Fact]
    public async Task TestFilterItemOptionalId()
    {
        var result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", new Dictionary<string, object?>
                    {
                        {
                            "items", new List<Dictionary<string, object>>
                            {
                                new()
                                {
                                    {
                                        "columnField", "firstname"
                                    },
                                    {
                                        "value", new MuiValue("Celeste")
                                    },
                                    {
                                        "operatorValue", "equals"
                                    },
                                    {
                                        "id", 1342532
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });
        result.Errors.Should().BeNull();

        result = await this.fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", new Dictionary<string, object?>
                    {
                        {
                            "items", new List<Dictionary<string, object>>
                            {
                                new()
                                {
                                    {
                                        "columnField", "firstname"
                                    },
                                    {
                                        "value", new MuiValue("Celeste")
                                    },
                                    {
                                        "operatorValue", "equals"
                                    },
                                    {
                                        "id", "1342532"
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });
        result.Errors.Should().BeNull();
    }

    [Fact]
    public async Task TestHttpStaticQuery()
    {
        var server = this.fixture.CreateTestServer();

        var request = new ClientQueryRequest
        {
            Query = @"query people { 
                people(filters: {
                    items: [{
                        columnField: ""firstname"",
                        value: ""Celeste"",
                        operatorValue: ""equals""
                    }, {
                        columnField: ""age"",
                        value: [""5"", ""6""],
                        operatorValue: ""isAnyOf""
                    }]
                }) { firstname } 
            }",
        };
        var result = await server.PostAsync(request);

        result.MatchSnapshot();
    }

    [Fact]
    public async Task TestHttpVariableQuery()
    {
        var server = this.fixture.CreateTestServer();

        var request = new ClientQueryRequest
        {
            Query = @"query people($filters: MuiDataGridFilterInput!) { 
                people(filters: $filters) { firstname } 
            }",
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
                                        "columnField", "firstname"
                                    },
                                    {
                                        "value", "Celeste"
                                    },
                                    {
                                        "operatorValue", "equals"
                                    },
                                },
                                new Dictionary<string, object>
                                {
                                    {
                                        "columnField", "age"
                                    },
                                    {
                                        "value", new List<object>
                                        {
                                            "5",
                                            "6",
                                        }
                                    },
                                    {
                                        "operatorValue", "isAnyOf"
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
}
