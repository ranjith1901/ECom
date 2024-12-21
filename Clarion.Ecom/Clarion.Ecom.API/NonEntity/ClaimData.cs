namespace Clarion.Ecom.API.NonEntity
{
    /// <summary>
    /// Claims data
    /// </summary>
    public class ClaimData
    {
        /// <summary>
        /// Gets or Sets User Id
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// Gets or Sets Company Id
        /// </summary>
        public long CompanyID { get; set; }
        /// <summary>
        /// Gets or Sets the Username
        /// </summary>
        public string Username { get; set; } = null!;
        /// <summary>
        /// Gets or Sets the User Role
        /// </summary>
        public long UserRoleID { get; set; }
        /// <summary>
        /// Gets or Sets the User Email
        /// </summary>
        public string UserEmail { get; set; } = null!;

    }


}
