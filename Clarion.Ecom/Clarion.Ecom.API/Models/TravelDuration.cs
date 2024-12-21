using System;
using System.Collections.Generic;

namespace Clarion.Ecom.API.Models;

public partial class TravelDuration
{
    public long DurationID { get; set; }

    public string DurationName { get; set; } = null!;

    public string? Remarks { get; set; }

    public long CompanyID { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public long? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public int DurationStatus { get; set; }
}
