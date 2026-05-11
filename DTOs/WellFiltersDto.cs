namespace WellsAPI.DTOs
{
    public class WellFiltersDto
    {
        public List<string> States { get; set; } = new();
        public List<string> Basins { get; set; } = new();
        public List<string> Statuses { get; set; } = new();
    }
}
