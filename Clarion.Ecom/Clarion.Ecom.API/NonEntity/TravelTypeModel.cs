namespace Clarion.Ecom.API.NonEntity
{
    public class TravelTypeModel
    {
        public long TravelTypeID { get; set; }

        public string? TravelTypeName { get; set; }

        public string? Remarks { get; set; }

        public long? CompanyID { get; set; }

        public int? TravelTypeStatus { get; set; }
    }
}
