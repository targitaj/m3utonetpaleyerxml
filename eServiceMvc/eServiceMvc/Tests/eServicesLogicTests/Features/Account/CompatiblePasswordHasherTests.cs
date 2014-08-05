namespace Uma.Eservices.LogicTests
{
    using FluentAssertions;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Logic.Features.Account;

    [TestClass]
    public class CompatiblePasswordHasherTests
    {
        [TestMethod]
        public void CompPswHasherOldPasswordIsValidated()
        {
            string password = "Parole123";
            string passwordHash = "1ehCx5Hrf+jFerHOYxcc/dJL1rw=";
            string passwordSalt = "ZDuD1AHT6zA2v9AWHof7fA==";
            string passwordType = "1";

            var hasher = new CompatiblePasswordHasher();
            var response =
                hasher.VerifyHashedPassword(
                    string.Format("{0}|{1}|{2}", passwordHash, passwordType, passwordSalt),
                    password);
            response.Should().Be(PasswordVerificationResult.SuccessRehashNeeded);
        }

        [TestMethod]
        public void CompPswHasherNewPasswordIsValidated()
        {
            string password = "Parole123";
            string passwordHash = "AC+tJ8TKBFjaPAbHVU6mNyiqRI9VrORSeDkHCiO1mhhGU8l0hagUjKHfwVxHFw8haw==";

            var hasher = new CompatiblePasswordHasher();
            var response =
                hasher.VerifyHashedPassword(passwordHash, password);
            response.Should().Be(PasswordVerificationResult.Success);
        }

        [TestMethod]
        public void CompPswHasherOldPasswordWrongIsInvalidated()
        {
            string password = "WrongPass";
            string passwordHash = "1ehCx5Hrf+jFerHOYxcc/dJL1rw=";
            string passwordSalt = "ZDuD1AHT6zA2v9AWHof7fA==";
            string passwordType = "1";

            var hasher = new CompatiblePasswordHasher();
            var response =
                hasher.VerifyHashedPassword(
                    string.Format("{0}|{1}|{2}", passwordHash, passwordType, passwordSalt),
                    password);
            response.Should().Be(PasswordVerificationResult.Failed);
        }

        [TestMethod]
        public void CompPswHasherNewPasswordWrongIsInvalidated()
        {
            string password = "WrongPass";
            string passwordHash = "AC+tJ8TKBFjaPAbHVU6mNyiqRI9VrORSeDkHCiO1mhhGU8l0hagUjKHfwVxHFw8haw==";

            var hasher = new CompatiblePasswordHasher();
            var response =
                hasher.VerifyHashedPassword(passwordHash, password);
            response.Should().Be(PasswordVerificationResult.Failed);
        }
    }
}
