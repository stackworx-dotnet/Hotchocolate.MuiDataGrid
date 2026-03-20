# Hotchocolate MuiDataGrid

## Setup

`Stackworx.Hotchocolate.MuiDataGrid` builds filter/sort expressions from MUI DataGrid model payloads.

1. Add a reference to the library.

```xml
<ItemGroup>
  <ProjectReference Include="src/Hotchocolate.MuiDataGrid/Hotchocolate.MuiDataGrid.csproj" />
</ItemGroup>
```

2. Register the MUI scalar/enum types with Hot Chocolate.

```csharp
builder
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMuiDataGrid();
```

This repository targets `net9.0`.

## Experimental MudBlazor integration

An adapter is available in `src/Hotchocolate.MudDataGrid` to translate MudBlazor DataGrid state into the MUI filter/sort contract used by `ExpressionBuilder<T>`.

Register it alongside `AddMuiDataGrid()`:

```csharp
builder
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMuiDataGrid()
    .AddMudDataGridAdapter();
```

For full details (input shape, operator mapping, resolver example), see `src/Hotchocolate.MudDataGrid/README.md`.

## Testing

Run the integration tests from the repo root:

```bash
./scripts/tests.sh
```

Or run the test project directly:

```bash
dotnet test ./tests/Hotchocolate.MuiDataGrid.Test --configuration Release
```

## Usage

The backend accepts `MuiDataGridFilterInput` and sort items from your GraphQL query, then applies them with `ExpressionBuilder<T>`.

### Sample column mapper

Map MUI field names (for example `firstname`) to entity members.

```csharp
using Stackworx.Hotchocolate.MuiDataGrid;

public sealed class PersonColumnLookup : BaseColumnLookup<Person>
{
    protected override ColumnLookupMember? InternalLookup(string column)
    {
        return column switch
        {
            "id" => this.GetMemberExpression(p => p.Id),
            "firstname" => this.GetMemberExpression(p => p.Firstname),
            "lastname" => this.GetMemberExpression(p => p.Lastname),
            "age" => this.GetMemberExpression(p => p.Age),
            "createdAtDate" => this.GetMemberExpression(p => p.CreatedAtDate),
            "apartmentType" => this.GetMemberExpression(p => p.Address!.Apartment.ApartmentType),
            _ => null,
        };
    }
}
```

### Sample resolver

Create one `ExpressionBuilder<T>` for the resolver, register custom handlers where needed, and apply filter/sort.

```csharp
using Microsoft.EntityFrameworkCore;
using Stackworx.Hotchocolate.MuiDataGrid;

public class Query
{
    public async Task<List<Person>> People(
        MuiDataGridFilterInput? filters,
        IList<MuiDataGridSortItem>? sorting,
        AppDbContext dbContext)
    {
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());

        // Optional: map non-default types/operators to a specific handler.
        builder.AddHandler("apartmentType", new DefaultEnumSingleSelectHandler<Person, ApartmentType>());

        IQueryable<Person> query = dbContext.People;

        if (filters is not null)
        {
            query = query.Where(builder.Filter(filters));
        }

        if (sorting is not null)
        {
            query = builder.Sort(query, sorting);
        }

        return await query.ToListAsync();
    }
}
```

### GraphQL request shape

```graphql
query People($filters: MuiDataGridFilterInput, $sorting: [MuiDataGridSortItemInput!]) {
  people(filters: $filters, sorting: $sorting) {
    id
    firstname
    lastname
  }
}
```

```json
{
  "filters": {
    "logicOperator": "and",
    "items": [
      { "field": "firstname", "operator": "contains", "value": "ann" },
      { "field": "age", "operator": ">=", "value": 18 }
    ]
  },
  "sorting": [
    { "field": "createdAtDate", "sort": "desc" }
  ]
}
```

## Operators

Not all MUI operators are supported for all default handlers.

### String (`DefaultStringHandler`)

`equals`, `contains`, `startsWith`, `endsWith`, `isEmpty`, `isNotEmpty`, `isAnyOf`

### Number (`DefaultNumberHandler`)

`=`, `!=`, `>`, `>=`, `<`, `<=`, `isEmpty`, `isNotEmpty`, `isAnyOf`

### Boolean (`DefaultBooleanHandler`)

`is`

### Date/DateTime (`DefaultDateOnlyHandler`, `DefaultDateTimeHandler`)

`is`, `not`, `after`, `onOrAfter`, `before`, `onOrBefore`, `isEmpty`, `isNotEmpty`

### Guid

```javascript
getGridSingleSelectOperators().filter(({ value }) =>
  ['equals', 'isEmpty', 'isNotEmpty', 'isAnyOf'].includes(value)
);
```