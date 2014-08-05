namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// View Model for Change Password screen
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// User's old password
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// User's entered new password
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// User's new password reconfirmation
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
