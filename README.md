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

## Migrating from the previous version

This release contains a breaking API change: `ExpressionBuilder<T>` now requires a `DataType<T>` instead of a `BaseColumnLookup<T>`, and custom handlers are configured on the data type rather than registered later on the builder.

### What changed

- `new ExpressionBuilder<T>(new PersonColumnLookup())` → `new ExpressionBuilder<T>(new PersonDataType())`
- `BaseColumnLookup<T>` mappings move into `DataType<T>.Configure(...)`
- `builder.AddHandler(...)` is removed; use `.SetHandler(...)` on the mapped property instead
- field names still default to camelCase property names, and `SetName(...)` is available to preserve old field contracts

### Old style

```csharp
public sealed class PersonColumnLookup : BaseColumnLookup<Person>
{
    protected override ColumnLookupMember? InternalLookup(string column)
    {
        return column switch
        {
            "firstname" => this.GetMemberExpression(p => p.Firstname),
            "apartmentType" => this.GetMemberExpression(p => p.Address!.Apartment.ApartmentType),
            _ => null,
        };
    }
}

var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
builder.AddHandler("apartmentType", new DefaultEnumSingleSelectHandler<Person, ApartmentType>());
```

### New style

```csharp
public sealed class PersonDataType : DataType<Person>
{
    protected override void Configure(DataTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Firstname);

        builder.Property(p => p.Address!.Apartment.ApartmentType)
            .SetName("apartmentType")
            .SetHandler(new DefaultEnumSingleSelectHandler<Person, ApartmentType>());
    }
}

var builder = new ExpressionBuilder<Person>(new PersonDataType());
```

### Migration checklist

1. Replace each `BaseColumnLookup<T>` with a `DataType<T>`.
2. Move each `switch` case into `builder.Property(...)` calls.
3. Add `SetName(...)` where the old field name does not match the inferred property name.
4. Move each `builder.AddHandler(...)` call into the corresponding property configuration via `SetHandler(...)`.
5. Update every `ExpressionBuilder<T>` construction site to pass the new data type.

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

### Sample data type

Map MUI field names (for example `firstname`) to entity members with `DataType<T>`. By default, fields are inferred from property names using camelCase, and you can override names with `SetName(...)` during transitions.

```csharp
using Stackworx.Hotchocolate.MuiDataGrid;

public sealed class PersonDataType : DataType<Person>
{
    protected override void Configure(DataTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Id);
        builder.Property(p => p.Firstname);
        builder.Property(p => p.Lastname);
        builder.Property(p => p.Age);
        builder.Property(p => p.CreatedAtDate);
        builder.Property(p => p.Address!.Apartment.ApartmentType)
            .SetName("apartmentType")
            .SetHandler(new DefaultEnumSingleSelectHandler<Person, ApartmentType>());
    }
}
```

### Sample resolver

Create one `ExpressionBuilder<T>` for the resolver from your configured `DataType<T>` and apply filter/sort.

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
        var builder = new ExpressionBuilder<Person>(new PersonDataType());

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