namespace Clarion.Ecom.API.NonEntity
{
    public class TravelDurationModel
    {
        public long DurationID { get; set; }

        public string DurationName { get; set; } = null!;

        public string? Remarks { get; set; }

        public long? CompanyID { get; set; }

        public int DurationStatus { get; set; }
    }
}
