namespace Stackworx.Hotchocolate.MuiDataGrid;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Stackworx.Hotchocolate.Muidatagrid.Entities;

public class MuiDataGridDbContextFactory : IDesignTimeDbContextFactory<MuiDataGridDbContext>
{
    public MuiDataGridDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MuiDataGridDbContext>();
        optionsBuilder.ConfigureDbContext();

        return new MuiDataGridDbContext(optionsBuilder.Options);
    }
}