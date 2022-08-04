namespace Stackworx.Hotchocolate.MuiDataGrid.GraphQL;

using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public class Query
{
    [UseDbContext(typeof(MuiDataGridDbContext))]
    public async Task<List<Person>> People(
        MuiDataGridFilterInput? filters,
        IList<MuiDataGridSortItem>? sorting,
        [ScopedService] MuiDataGridDbContext dbContext)
    {
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        builder.AddHandler("apartmentType", new DefaultEnumSingleSelectHandler<Person, ApartmentType>());

        IQueryable<Person> q = dbContext.People;
        if (filters != null)
        {
            q = q.Where(builder.Filter(filters));
        }

        if (sorting != null)
        {
            q = builder.Sort(q, sorting);
        }

        var res = await q.ToListAsync();

        return res;
    }

    // Endpoint for testing mui value serialization.
    public string? Input(
        MuiDataGridFilterItemInput input)
    {
        return input.Value?.ToString();
    }
}
