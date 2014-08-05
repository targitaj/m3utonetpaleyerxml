namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// View Model for Reset Password (through Forgot password) screen
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Email address of an account
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Field to hold Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Field to handle Confirm password
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Validation code (which is sent via e-mail to get to reset password screen
        /// </summary>
        public string Code { get; set; }
    }
}
