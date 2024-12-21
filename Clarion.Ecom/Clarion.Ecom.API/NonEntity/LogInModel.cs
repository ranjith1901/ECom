using System.Globalization;

namespace Clarion.Ecom.API.NonEntity
{
    /// <summary>
    /// Model for LogIn Inputs
    /// </summary>
    public class LogInModel
    {
        /// <summary>
        /// user name
        /// </summary>
        public required string Username { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public required string Password { get; set; }
    }
    public class LoginRolePrivilege
    {
        public long PrivilegeID { get; set; }
        public string PrivilegeName { get; set; }
        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }

        public bool CanPrint { get; set; }

        public bool CanExport { get; set; }
    }
    /// <summary>
    /// Model for LogIn Response Details
    /// </summary>
    public class LogInResponse
    {
        /// <summary>
        /// User ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// Username 
        /// </summary>
        public required string UserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        ///Role ID
        /// </summary>
        public long RoleID { get; set; }
        /// <summary>
        /// StoreID
        /// </summary>
        public long CompanyID { get; set; }
        /// <summary>
        /// contact number
        /// </summary>
        public string? MobileNo { get; set; }
        /// <summary>
        /// Gets or Sets the User logged in using Temp Password
        /// </summary>
        public bool IsTempPasswordLogin { get; set; }
        /// <summary>
        /// UserPrivileges
        /// </summary>
        public ICollection<LoginRolePrivilege> RolePrivileges { get; set; } = new List<LoginRolePrivilege>();
        /// <summary>
        /// Date format
        /// </summary>
        public string DateFormat { get; set; } = string.Empty;
        /// <summary>
        /// Time format
        /// </summary>
        public string TimeFormat { get; set; } = string.Empty;
        /// <summary>
        /// Currency value
        /// </summary>
        public string CurrencyValue { get; set; } = string.Empty;
        /// <summary>
        /// Auth Token
        /// </summary>
        public string Token { get; set; } = null!;
    }
    /// <summary>
    /// Reset Password Model
    /// </summary>
    public class SetPasswordModel
    {
        /// <summary>
        /// UserID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// NewPassword
        /// </summary>
        public string NewPassword { get; set; } = null!;
        /// <summary>
        /// ConfirmPassword
        /// </summary>
        public string ConfirmPassword { get; set; } = null!;
    }
    /// <summary>
    /// Update/Reset Password
    /// </summary>
    public class UpdatePassword
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CurrentPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public required string NewPassword { get; set; }
    }
}
