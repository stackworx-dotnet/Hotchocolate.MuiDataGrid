# Hotchocolate.MudDataGrid

Adapter layer that translates a MudBlazor `GridState`-style input into the `MuiDataGrid` filter/sort contract used by `ExpressionBuilder<T>`.

## MudBlazor source definitions

```csharp
public List<IFilterDefinition<T>> FilterDefinitions { get; set; } = new List<IFilterDefinition<T>>();
public Dictionary<string, SortDefinition<T>> SortDefinitions { get; set; } = new Dictionary<string, SortDefinition<T>>();
```

## Setup

Register the adapter with your HotChocolate server alongside `AddMuiDataGrid`:

```csharp
services
    .AddGraphQL()
    .AddMuiDataGrid()
    .AddMudDataGridAdapter();
```

## Migrating from the previous version

If you were previously constructing the resolver with a `BaseColumnLookup<T>`, update it to use a `DataType<T>` instead:

```csharp
// old
var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());

// new
var builder = new ExpressionBuilder<Person>(new PersonDataType());
```

If the old resolver also registered handlers after construction, move that configuration into `PersonDataType.Configure(...)` by using `SetHandler(...)` on the relevant property. Use `SetName(...)` when you need to preserve an existing field name exposed to the Mud/MUI payload.

## GraphQL input types

`MudDataGridFilterInput` mirrors the flat shape sent from Blazor over the wire:

```graphql
input MudDataGridFilterInput {
  filterDefinitions: [MudDataGridFilterDefinitionInput!]!
  sortDefinitions:   [MudDataGridSortDefinitionInput!]!
}

input MudDataGridFilterDefinitionInput {
  field:    String!
  operator: String!
  value:    MuiValue
  id:       Any
}

input MudDataGridSortDefinitionInput {
  field:      String!
  descending: Boolean!
}
```

`sortDefinitions` is a **list** (not a dictionary) to preserve insertion order across GraphQL transport.

## Usage in a resolver

Inject `IMudToMuiDataGridAdapter` and call `Map`. Both `Filters` and `Sorting` on the result are nullable — they are `null` when the corresponding collection is empty, so no filtering/sorting is applied.

```csharp
public async Task<List<Person>> People(
    MudDataGridFilterInput? state,
    IMudToMuiDataGridAdapter adapter,      // injected from DI
    MuiDataGridDbContext dbContext)
{
    var builder = new ExpressionBuilder<Person>(new PersonDataType());

    var mapped = adapter.Map(state);

    IQueryable<Person> q = dbContext.People;

    if (mapped.Filters is not null)
        q = q.Where(builder.Filter(mapped.Filters));

    if (mapped.Sorting is not null)
        q = builder.Sort(q, mapped.Sorting);

    return await q.ToListAsync();
}
```

## Operator mapping

Operators are matched **case-insensitively** after trimming. The table below shows every accepted Mud operator and the internal MUI operator it produces:

| Mud operator (case-insensitive) | MUI operator   | Applicable handlers |
|----------------------------------|----------------|---------------------|
| `contains`                       | `contains`     | String              |
| `starts with`                    | `startsWith`   | String              |
| `ends with`                      | `endsWith`     | String              |
| `equal` / `equals`               | `equals`       | String, Guid        |
| `=`                              | `=`            | Number              |
| `!=`                             | `!=`           | Number              |
| `>`                              | `>`            | Number              |
| `>=`                             | `>=`           | Number              |
| `<`                              | `<`            | Number              |
| `<=`                             | `<=`           | Number              |
| `is`                             | `is`           | Boolean, DateTime, SingleSelect |
| `is not` / `not`                 | `not`          | DateTime, SingleSelect |
| `empty`                          | `isEmpty`      | String, Number, DateTime, Guid |
| `not empty`                      | `isNotEmpty`   | String, Number, DateTime, Guid |
| `any of` / `is any of`           | `isAnyOf`      | String, Number, Guid, SingleSelect |

Any operator not listed above throws a `GraphQLException` immediately (fail-fast).

## Error handling

Unsupported operators are surfaced as a structured GraphQL error before the resolver reaches `ExpressionBuilder<T>`:

```json
{
  "errors": [
    {
      "message": "MudDataGrid operator 'not contains' is not supported for field 'firstname'.",
      "extensions": {
        "code": "MUD_OPERATOR_NOT_SUPPORTED",
        "field": "firstname",
        "operator": "not contains"
      }
    }
  ]
}
```

## References

- https://github.com/MudBlazor/MudBlazor/blob/v9.2.0/src/MudBlazor/Components/DataGrid/GridState.cs
- https://github.com/MudBlazor/MudBlazor/blob/v9.2.0/src/MudBlazor/Components/DataGrid/FilterOperator.cs
- https://github.com/MudBlazor/MudBlazor/blob/v9.2.0/src/MudBlazor/Components/DataGrid/SortDefinition.cs
- https://github.com/MudBlazor/MudBlazor/blob/v9.2.0/src/MudBlazor/Components/DataGrid/Definition/IFilterDefinition.cs