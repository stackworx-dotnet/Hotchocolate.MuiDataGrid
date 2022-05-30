namespace Stackworx.Hotchocolate.MuiDataGrid.GraphQL;

using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using Stackworx.Hotchocolate.MuiDataGrid;
using Stackworx.Hotchocolate.Muidatagrid.Entities;
using Stackworx.Hotchocolate.Muidatagrid.GraphQL;

public class Query
{
    [UseDbContext(typeof(MuiDataGridDbContext))]
    public async Task<List<Person>> People(
        MuiDataGridFilterInput filters,
        [ScopedService] MuiDataGridDbContext dbContext)
    {
        var builder = new ExpressionBuilder<Person>(new PersonColumnLookup());
        // builder.AddHandler();
        var q = dbContext.People.Where(builder.Filter(filters));

        var s = q.ToQueryString();

        return await q.ToListAsync();
    }
}