namespace Stackworx.Hotchocolate.MudDataGrid;

public interface IMudToMuiDataGridAdapter
{
    MudDataGridAdapterResult Map(MudDataGridFilterInput? input);
}