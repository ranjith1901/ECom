namespace Clarion.Ecom.Web.Models
{
    /// <summary>
    /// App Settings
    /// </summary>
    public class AppSettings
    {
        private string _apiUrl = string.Empty;
        /// <summary>
        /// Gets or Sets the APIUrl
        /// </summary>
        public string APIUrl
        {
            get { return _apiUrl; }
            set { _apiUrl = value; }
        }
    }
}
