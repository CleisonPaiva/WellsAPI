namespace WellsAPI.DTOs
{
    public class WellResponseDto
    {
        public string? Name { get; set; }
        public string? NameWellOperator { get; set; }
        public string? OrganizationNumber { get; set; }
        public string? Operator { get; set; }
        public string? State { get; set; }

        public string? Basin { get; set; }
        public string? Status { get; set; }

        public string? Classification { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
