namespace Stackworx.Hotchocolate.MuiDataGrid;

using System.Collections.Immutable;
using FluentAssertions;
using HotChocolate;
using HotChocolate.Execution;
using Snapshooter.Xunit;

[Collection(nameof(DbFixtureCollection))]
public partial class MuiDataGridGraphQLTests(DbFixture fixture)
{
    [Fact]
    public void TestSchema()
    {
        fixture.RequestExecutor.Schema.ToString().MatchSnapshot();
    }

    [Fact]
    public async Task TestOrLinkOperatorExecute()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(
                        new MuiDataGridFilterInput
                        {
                            Items = new List<MuiDataGridFilterItemInput>
                            {
                                new(
                                    Field: "firstname",
                                    Value: new MuiValue("\"Celeste\""),
                                    Operator: "equals"),
                                new(
                                    Field: "firstname",
                                    Value: new MuiValue("\"Johanny\""),
                                    Operator: "equals"),
                            },
                            LogicOperator = MuiDataGridLogicOperator.Or,
                        })
                },
            });
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task TestAndLinkOperatorExecute()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(
                        new MuiDataGridFilterInput
                        {
                            Items = new List<MuiDataGridFilterItemInput>
                            {
                                new(
                                    Field: "firstname",
                                    Value: new MuiValue("Celeste"),
                                    Operator: "equals"),
                                new(
                                    Field: "lastname",
                                    Value: new MuiValue("Le Roux"),
                                    Operator: "equals"),
                            },
                            LogicOperator = MuiDataGridLogicOperator.And,
                        })
                },
            });
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task TestChainFiltersTogetherExecute()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname, lastname, gender, dateOfBirth } }",
            new Dictionary<string, object?>
            {
                {
                    "filters", this.ConvertFilterInput(
                        new MuiDataGridFilterInput
                        {
                            Items = new List<MuiDataGridFilterItemInput>
                            {
                                new(
                                    Field: "firstname",
                                    Value: new MuiValue("Celeste"),
                                    Operator: "equals"),
                                new(
                                    Field: "lastname",
                                    Value: new MuiValue("Le Roux"),
                                    Operator: "equals"),
                                // new(
                                //     Field: "gender",
                                //     Value: new MuiValue("MALE"),
                                //     Operator: "is"),
                                // TODO: this breaks on linux
                                /*
                                new(
                                    Field: "dateOfBirth",
                                    Value: new MuiValue("2022-05-27T13:41"),
                                    Operator: "after"),
                                */
                                new(
                                    Field: "marriageDate",
                                    Value: null,
                                    Operator: "isNotEmpty"),
                            },
                            LogicOperator = MuiDataGridLogicOperator.And,
                            QuickFilterLogicOperator = MuiDataGridLogicOperator.And,
                            QuickFilterValues = new MuiValue([]),
                        })
                },
            });
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task TestPersonQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people { people { firstname } }");
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
        result.ToJson().MatchSnapshot();
    }

    [Fact]
    public async Task TestPersonAddressNestedQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
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
                                        "field", "apartmentName"
                                    },
                                    {
                                        "value", new MuiValue("Number")
                                    },
                                    {
                                        "operator", "contains"
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

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestDateTimeNullableQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname address { apartment { sellDate } } } }",
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
                                        "field", "sellDate"
                                    },
                                    {
                                        "value", new MuiValue("2022-07-08")
                                    },
                                    {
                                        "operator", "is"
                                    },
                                    {
                                        "id", 1342530
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestDecimalQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
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
                                        "field", "price"
                                    },
                                    {
                                        "value", new MuiValue("14.01")
                                    },
                                    {
                                        "operator", ">="
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

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestNullableDecimalQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { bankAccountBalance } }",
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
                                        "field", "bankAccountBalance"
                                    },
                                    {
                                        "value", new MuiValue("10.51")
                                    },
                                    {
                                        "operator", ">="
                                    },
                                    {
                                        "id", 1842790
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestNullableDoubleQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { weight } }",
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
                                        "field", "weight"
                                    },
                                    {
                                        "value", new MuiValue("70.02")
                                    },
                                    {
                                        "operator", ">="
                                    },
                                    {
                                        "id", 1942723
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestRelayIdIntQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { id } }",
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
                                        "field", "id"
                                    },
                                    {
                                        "value", new MuiValue(new DefaultNodeIdSerializer().Format("Person", 1))
                                    },
                                    {
                                        "operator", "is"
                                    },
                                    {
                                        "id", 134253732
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestRelayIdGuidQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { refId } }",
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
                                        "field", "refId"
                                    },
                                    {
                                        "value", new MuiValue(
                                            new DefaultNodeIdSerializer().Format(
                                                "Ref",
                                                Guid.Parse("9F1EF691-2C4B-4BDE-B0AC-635BDD4E180C")))
                                    },
                                    {
                                        "operator", "is"
                                    },
                                    {
                                        "id", 1425372
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestSingleSelectEnumWithQuery()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
            "query people($filters: MuiDataGridFilterInput!) { people(filters: $filters) { firstname address { apartment { apartmentType } } } }",
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
                                        "field", "apartmentType"
                                    },
                                    {
                                        "value", new MuiValue("RENTAL_PROPERTY")
                                    },
                                    {
                                        "operator", "is"
                                    },
                                    {
                                        "id", 19216
                                    },
                                },
                            }
                        },
                    }.ToImmutableDictionary()
                },
            });

        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task TestFilterItemOptionalId()
    {
        var result = await fixture.RequestExecutor.ExecuteAsync(
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
                                        "field", "firstname"
                                    },
                                    {
                                        "value", new MuiValue("Celeste")
                                    },
                                    {
                                        "operator", "equals"
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
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();

        result = await fixture.RequestExecutor.ExecuteAsync(
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
                                        "field", "firstname"
                                    },
                                    {
                                        "value", new MuiValue("Celeste")
                                    },
                                    {
                                        "operator", "equals"
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
        result.ExpectOperationResult().Errors.Should().BeNullOrEmpty();
    }
}