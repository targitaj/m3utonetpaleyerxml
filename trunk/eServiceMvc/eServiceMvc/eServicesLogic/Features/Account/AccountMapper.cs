namespace Uma.Eservices.Logic.Features.Account
{
    using Uma.Eservices.Models.Account;

    public static class AccountMapper
    {
        /// <summary>
        /// Maps ViewModel WebUser object to UserProfileModel object.
        /// </summary>
        /// <param name="webUser">ViewModel WebUser object</param>
        /// <returns>UserProfileModel object</returns>
        public static UserProfileModel ToUserProfileModel(this WebUser webUser)
        {
            if (webUser == null)
            {
                return null;
            }

            return new UserProfileModel
                       {
                           FirstName = webUser.FirstName,
                           LastName = webUser.LastName,
                           BirthDate = webUser.BirthDate,
                           PersonCode = webUser.PersonCode,
                           Email = webUser.UserName,
                           Mobile = webUser.Mobile
                       };
        }

        /// <summary>
        /// Converts UserProfileModel object to WebUser model object
        /// </summary>
        /// <param name="webUser">WebUser object</param>
        /// <param name="model">UserProfileModel object</param>
        public static WebUser FromUserProfile(this WebUser webUser, UserProfileModel model)
        {
            if (model == null)
            {
                return webUser;
            }

            webUser.BirthDate = model.BirthDate;
            webUser.FirstName = model.FirstName;
            webUser.LastName = model.LastName;
            webUser.Mobile = model.Mobile;
            webUser.UserName = model.Email;
            webUser.PersonCode = model.PersonCode;

            return webUser;
        }
    }
}
