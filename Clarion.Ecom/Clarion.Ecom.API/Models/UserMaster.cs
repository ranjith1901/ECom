using System;
using System.Collections.Generic;

namespace Clarion.Ecom.API.Models;

public partial class UserMaster
{
    public long UserID { get; set; }

    public long CompanyID { get; set; }

    public long UserRoleID { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string LoginName { get; set; } = null!;

    public string LoginPassword { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Remarks { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public long LastModifiedBy { get; set; }

    public DateTime LastModifiedOn { get; set; }

    public int UserStatus { get; set; }
}
