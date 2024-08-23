namespace Olympics.Web.Grid;

public interface IMedalFilters
{
    MedalFilterColumns FilterColumn { get; set; }
    MedalFilterColumns SortColumn { get; set; }
    bool Loading { get; set; }
    string? FilterText { get; set; }
    bool SortAscending { get; set; }
}
