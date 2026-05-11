namespace WellsAPI.DTOs
{
    public class WellFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Name { get; set; }
        public string? NameWellOperator { get; set; }
        public string? OrganizationNumber { get; set; }
        public string? Operator { get; set; }
        public string? State { get; set; }

        public string? Basin { get; set; }
        public string? Status { get; set; }

        public string? Classification { get; set; }

        public decimal? MinLongitude { get; set; }
        public decimal? MaxLongitude { get; set; }
        public decimal? MinLatitude { get; set; }
        public decimal? MaxLatitude { get; set; }
        public int Zoom { get; set; }
    }
}
