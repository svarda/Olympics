namespace Olympics.Web.Grid;

public class GridControls : IMedalFilters {
    public string? FilterText { get; set; }
    public MedalFilterColumns FilterColumn { get; set; } = MedalFilterColumns.Year;
    public MedalFilterColumns SortColumn { get; set; } = MedalFilterColumns.Year;
    public bool SortAscending { get; set; } = true;
    public bool Loading { get; set; }
}
