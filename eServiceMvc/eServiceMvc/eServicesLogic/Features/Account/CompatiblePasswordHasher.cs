namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Override of standard included Hashing mechanism to handle legacy users with Membership passwords
    /// http://www.asp.net/identity/overview/migrations/migrating-an-existing-website-from-sql-membership-to-aspnet-identity
    /// </summary>
    public class CompatiblePasswordHasher : PasswordHasher
    {
        /// <summary>
        /// Verifies the hashed password. Handles converted passwords from Membership provider and new Identity passwords
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="providedPassword">The provided password.</param>
        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                return PasswordVerificationResult.Failed;
            }

            if (providedPassword == null)
            {
                throw new ArgumentNullException("providedPassword");
            }

            // Check if old password is supplied - it has 3 parts (from 3 fields in old password storage schema)
            string[] passwordProperties = hashedPassword.Split('|');
            if (passwordProperties.Length != 3)
            {
                return base.VerifyHashedPassword(hashedPassword, providedPassword);
            }

            string passwordHash = passwordProperties[0];
            string salt = passwordProperties[2];
            if (string.Equals(
                this.EncryptPassword(providedPassword, 1, salt),
                passwordHash,
                StringComparison.CurrentCultureIgnoreCase))
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }

            return PasswordVerificationResult.Failed;
        }

        /// <summary>
        /// Encrypts the password the way it was encrypted in old Membership provider.
        /// Copied from old SQL server code in MS site, so no need for testing (was tested by thousands of appliances all over the world).
        /// </summary>
        /// <param name="pass">The password.</param>
        /// <param name="passwordFormat">The password format (here always 1 - default for membership).</param>
        /// <param name="salt">The salt value.</param>
        [ExcludeFromCodeCoverage]
        private string EncryptPassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0)
            {
                // MembershipPasswordFormat.Clear
                return pass;
            }

            byte[] passwordBytes = Encoding.Unicode.GetBytes(pass);
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] returnBytes = null;

            if (passwordFormat == 1)
            {
                // MembershipPasswordFormat.Hashed 
                HashAlgorithm hashingAlgoritm = HashAlgorithm.Create("SHA1");
                KeyedHashAlgorithm hashAlgorithm = hashingAlgoritm as KeyedHashAlgorithm;
                if (hashAlgorithm != null)
                {
                    if (hashAlgorithm.Key.Length == saltBytes.Length)
                    {
                        hashAlgorithm.Key = saltBytes;
                    }
                    else if (hashAlgorithm.Key.Length < saltBytes.Length)
                    {
                        byte[] keyBytes = new byte[hashAlgorithm.Key.Length];
                        Buffer.BlockCopy(saltBytes, 0, keyBytes, 0, keyBytes.Length);
                        hashAlgorithm.Key = keyBytes;
                    }
                    else
                    {
                        byte[] keyBytes = new byte[hashAlgorithm.Key.Length];
                        for (int iter = 0; iter < keyBytes.Length;)
                        {
                            int len = Math.Min(saltBytes.Length, keyBytes.Length - iter);
                            Buffer.BlockCopy(saltBytes, 0, keyBytes, iter, len);
                            iter += len;
                        }
                        hashAlgorithm.Key = keyBytes;
                    }

                    returnBytes = hashAlgorithm.ComputeHash(passwordBytes);
                }
                else
                {
                    byte[] allBytes = new byte[saltBytes.Length + passwordBytes.Length];
                    Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
                    Buffer.BlockCopy(passwordBytes, 0, allBytes, saltBytes.Length, passwordBytes.Length);
                    returnBytes = hashingAlgoritm.ComputeHash(allBytes);
                }
            }

            return Convert.ToBase64String(returnBytes);
        }
    }
}