namespace Uma.Eservices.LogicTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.TestHelpers;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class IdentityStoreTests
    {
        private IIdentityStore<ApplicationUser> store;

        private IdentityUser fakeUserFromDb;

        private Mock<ISecurityDataHelper> dbHelperMock;

        [TestInitialize]
        public void StoreSetup()
        {
            Guid userIdentifier = Guid.NewGuid();
            this.fakeUserFromDb = new IdentityUser
                                      {
                                          Id = userIdentifier,
                                          EmailAddress = RandomData.GetEmailAddress(),
                                          PasswordHash =
                                              RandomData.GetString(18, 24, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Numbers) + "==",
                                          FirstName = RandomData.GetStringPersonFirstName(),
                                          LastName = RandomData.GetStringPersonLastName(),
                                          IsConfirmed = true,
                                          CreatedDate = RandomData.GetDateTimeInPast(),
                                          SecurityStamp = Guid.NewGuid().ToString("D"),
                                          Logins =
                                              new List<IdentityUserLogin>
                                                  {
                                                      new IdentityUserLogin
                                                          {
                                                              LoginProvider = "Yahoo",
                                                              ProviderKey = RandomData.GetStringNumber(18),
                                                              UserId = userIdentifier
                                                          }
                                                  },
                                          Roles = new List<IdentityUserRole>(),
                                          Claims =
                                              new List<IdentityUserClaim>
                                                  {
                                                      new IdentityUserClaim
                                                          {
                                                              ClaimId = RandomData.GetInteger(1000, 9999),
                                                              ClaimType = "FavoriteActress",
                                                              ClaimValue = "Sandra Bullock",
                                                              UserId = userIdentifier
                                                          }
                                                  }
                                      };
            List<IdentityUser> userList = new List<IdentityUser> { this.fakeUserFromDb };
            var role = new IdentityUserRole { RoleId = RandomData.GetInteger(1000, 9999), RoleName = RandomData.GetStringWord(), Users = userList };
            this.fakeUserFromDb.Roles.Add(role);
            this.dbHelperMock = new Mock<ISecurityDataHelper>();
            this.dbHelperMock.Setup(mock => mock.GetUserAggregate(It.IsAny<Expression<Func<IdentityUser, bool>>>()))
                .Returns<Expression<Func<IdentityUser, bool>>>(predicate => userList.FirstOrDefault(predicate.Compile()));
            this.dbHelperMock.Setup(mock => mock.GetById<IdentityUser, Guid>(It.IsAny<Guid>())).Returns((Guid userId) => userList.FirstOrDefault(u => u.Id == userId));
            this.store = new IdentityStore<ApplicationUser>(this.dbHelperMock.Object);
        }

        [TestMethod]
        public void IdentStoreCreateNullUserException()
        {
            Task result = this.store.CreateAsync(null);
            result.Exception.Should().NotBeNull();
            result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreCreateUserCallsDatabaseHelperCreate()
        {
            var appUser = new ApplicationUser();
            this.store.CreateAsync(appUser);
            this.dbHelperMock.Verify(db => db.Create(It.IsAny<IdentityUser>()), Times.Once());
        }

        [TestMethod]
        public void IdentStoreUpdateNullUserException()
        {
            var result = this.store.UpdateAsync(null);
            result.Exception.Should().NotBeNull();
            result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreUpdateUserCallsDatabaseHelperUpdate()
        {
            var appUser = new ApplicationUser();
            this.store.UpdateAsync(appUser);
            this.dbHelperMock.Verify(db => db.Update(It.IsAny<IdentityUser>()), Times.Once());
        }

        [TestMethod]
        public void IdentStoreDeleteNullUserException()
        {
            var result = this.store.DeleteAsync(null);
            result.Exception.Should().NotBeNull();
            result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreDeleteUserCallsDatabaseHelperDeleteById()
        {
            var appUser = new ApplicationUser { Id = Guid.NewGuid() };
            this.store.DeleteAsync(appUser);
            this.dbHelperMock.Verify(db => db.DeleteById<IdentityUser, Guid>(appUser.Id), Times.Once());
        }

        [TestMethod]
        public void IdentStoreGetPasswordHashReturnsUserStoredPasswordHash()
        {
            var appUser = new ApplicationUser { PasswordHash = RandomData.GetString(18, 24, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Numbers) };
            var result = this.store.GetPasswordHashAsync(appUser);
            result.Should().BeOfType<Task<string>>();
            result.Result.Should().Be(appUser.PasswordHash);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreGetPasswordNullUserThrowsExc()
        {
            var result = this.store.GetPasswordHashAsync(null);
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreSetPasswordNullUserThrowsExc()
        {
            var result = this.store.SetPasswordHashAsync(null, "DoesntMatter");
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreSetPasswordHashStoredInUserProp()
        {
            string passwordHash = RandomData.GetString(18, 24, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Numbers);
            var appUser = new ApplicationUser();
            var result = this.store.SetPasswordHashAsync(appUser, passwordHash);
            result.Should().BeOfType<Task<int>>();
            ((Task<int>)result).Result.Should().Be(0);
            appUser.PasswordHash.Should().Be(passwordHash);
        }

        [TestMethod]
        public void IdentStoreHasPasswordForMissingPasswordReturnsFalse()
        {
            var appUser = new ApplicationUser();
            var result = this.store.HasPasswordAsync(appUser);
            result.Should().BeOfType<Task<bool>>();
            result.Result.Should().Be(false);
        }

        [TestMethod]
        public void IdentStoreHasPasswordForexistingPasswordReturnsTrue()
        {
            string passwordHash = RandomData.GetString(18, 24, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Numbers);
            var appUser = new ApplicationUser { PasswordHash = passwordHash };
            var result = this.store.HasPasswordAsync(appUser);
            result.Should().BeOfType<Task<bool>>();
            result.Result.Should().Be(true);
        }

        [TestMethod]
        public void IdentStoreFindByIdReturnsExpectedUser()
        {
            var result = this.store.FindByIdAsync(this.fakeUserFromDb.Id);
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<ApplicationUser>();
            ApplicationUser resultUser = result.Result;
            resultUser.Id.Should().Be(this.fakeUserFromDb.Id);
            resultUser.UserName.Should().Be(this.fakeUserFromDb.EmailAddress);
        }

        [TestMethod]
        public void IdentStoreFindByRandomIdReturnsNull()
        {
            var result = this.store.FindByIdAsync(Guid.NewGuid());
            result.Result.Should().BeNull();
        }

        [TestMethod]
        public void IdentStoreFindByNameReturnsExpectedUser()
        {
            var result = this.store.FindByNameAsync(this.fakeUserFromDb.EmailAddress);
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType<ApplicationUser>();
            ApplicationUser resultUser = result.Result;
            resultUser.Id.Should().Be(this.fakeUserFromDb.Id);
            resultUser.UserName.Should().Be(this.fakeUserFromDb.EmailAddress);
        }

        [TestMethod]
        public void IdentStoreFindByRandomNameReturnsNull()
        {
            var result = this.store.FindByNameAsync(RandomData.GetStringWord());
            result.Result.Should().BeNull();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreAddLoginNullUserThrowsExc()
        {
            var result = this.store.AddLoginAsync(null, new UserLoginInfo("Facebook", "123123123123"));
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreAddLoginNullLoginThrowsExc()
        {
            var result = this.store.AddLoginAsync(new ApplicationUser(), null);
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreAddLoginCallsDatabase()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var result = this.store.AddLoginAsync(appUser, new UserLoginInfo("Facebook", RandomData.GetStringNumber(12)));
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Once());
            this.fakeUserFromDb.Logins.Count.Should().Be(2); // One existing and one added
        }

        [TestMethod]
        public void IdentStoreAddLoginAddsToAppUser()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var result = this.store.AddLoginAsync(appUser, new UserLoginInfo("Facebook", RandomData.GetStringNumber(12)));
            appUser.Logins.Count.Should().Be(1); // App user is not reloaded when adding.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreRemoveLoginNullUserThrowsExc()
        {
            var result = this.store.RemoveLoginAsync(null, new UserLoginInfo("Facebook", "123123123123"));
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreRemoveLoginNullLoginThrowsExc()
        {
            var result = this.store.RemoveLoginAsync(new ApplicationUser(), null);
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreRemoveLoginCallsDatabase()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var existingLogin = new UserLoginInfo("Yahoo", this.fakeUserFromDb.Logins.First().ProviderKey);
            var result = this.store.RemoveLoginAsync(appUser, existingLogin);
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Once());
            this.fakeUserFromDb.Logins.Count.Should().Be(0);
        }

        [TestMethod]
        public void IdentStoreRemoveWrongLoginLeavesAppUserIntact()
        {
            var appUser = IdentityMapper.MapAppUserFromDbUser(this.fakeUserFromDb);
            var result = this.store.RemoveLoginAsync(appUser, new UserLoginInfo("Facebook", RandomData.GetStringNumber(12)));
            appUser.Logins.Count.Should().Be(1);
        }

        [TestMethod]
        public void IdentStoreRemoveLoginRemovesInMemObject()
        {
            var appUser = IdentityMapper.MapAppUserFromDbUser(this.fakeUserFromDb);
            var existingLogin = new UserLoginInfo("Yahoo", this.fakeUserFromDb.Logins.First().ProviderKey);
            var result = this.store.RemoveLoginAsync(appUser, existingLogin);
            appUser.Logins.Count.Should().Be(0);
        }

        [TestMethod]
        public void IdentStoreGetLoginsRetrievesThemFromInMem()
        {
            var appUser = IdentityMapper.MapAppUserFromDbUser(this.fakeUserFromDb);
            var result = this.store.GetLoginsAsync(appUser);
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Never());
            result.Result.Should().NotBeNull();
            result.Result.Count.Should().Be(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreGetLoginNullUserThrowsExc()
        {
            var result = this.store.GetLoginsAsync(null);
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreFindByLoginNullLoginThrowsExc()
        {
            var result = this.store.FindAsync(null);
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreFindByLoginCallsDatabaseForLogin()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var existingLogin = new UserLoginInfo("Bamberbia", "kergudu");
            List<IdentityUserLogin> loginList = new List<IdentityUserLogin>();
            loginList.AddRange(this.fakeUserFromDb.Logins);
            this.dbHelperMock.Setup(mock => mock.Get<IdentityUserLogin>(It.IsAny<Expression<Func<IdentityUserLogin, bool>>>()))
                .Returns<Expression<Func<IdentityUserLogin, bool>>>(predicate => loginList.FirstOrDefault<IdentityUserLogin>(predicate.Compile()));
            var result = this.store.FindAsync(existingLogin);
            this.dbHelperMock.Verify(m => m.Get<IdentityUserLogin>(It.IsAny<Expression<Func<IdentityUserLogin, bool>>>()), Times.Once());
            result.Should().BeNull();
        }

        [TestMethod]
        public void IdentStoreFindByLoginCallsDatabaseForUser()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var existingLogin = new UserLoginInfo("Yahoo", this.fakeUserFromDb.Logins.First().ProviderKey);
            List<IdentityUserLogin> loginList = new List<IdentityUserLogin>();
            loginList.AddRange(this.fakeUserFromDb.Logins);
            this.dbHelperMock.Setup(mock => mock.Get<IdentityUserLogin>(It.IsAny<Expression<Func<IdentityUserLogin, bool>>>()))
                .Returns<Expression<Func<IdentityUserLogin, bool>>>(predicate => loginList.FirstOrDefault<IdentityUserLogin>(predicate.Compile()));
            var result = this.store.FindAsync(existingLogin);
            this.dbHelperMock.Verify(m => m.GetUserAggregate(It.IsAny<Expression<Func<IdentityUser, bool>>>()), Times.Once());
            result.Result.Should().NotBeNull();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreGetClaimsForNullUserThrowsExc()
        {
            var result = this.store.GetClaimsAsync(null);
            // result.Exception.Should().NotBeNull();
            // result.Exception.InnerException.Should().BeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void IdentStoreGetClaimsRetrievesThemFromInMem()
        {
            var appUser = IdentityMapper.MapAppUserFromDbUser(this.fakeUserFromDb);
            var result = this.store.GetClaimsAsync(appUser);
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Never());
            result.Result.Should().NotBeNull();
            result.Result.Count.Should().Be(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreAddClaimNullUserThrowsExc()
        {
            var result = this.store.AddClaimAsync(null, new Claim("Bamberbia", "Kergudu"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreAddClaimNullClaimThrowsExc()
        {
            var result = this.store.AddClaimAsync(new ApplicationUser(), null);
        }

        [TestMethod]
        public void IdentStoreAddClaimCallsDatabase()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var result = this.store.AddClaimAsync(appUser, new Claim("Joparesete", RandomData.GetStringNumber(12)));
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Once());
            this.fakeUserFromDb.Claims.Count.Should().Be(2); // One existing and one added
        }

        [TestMethod]
        public void IdentStoreAddClaimAddsToAppUser()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var result = this.store.AddClaimAsync(appUser, new Claim("ShishKebab", RandomData.GetStringNumber(12)));
            appUser.Claims.Count.Should().Be(1); // App user is not reloaded when adding.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreRemoveClaimNullUserThrowsExc()
        {
            var result = this.store.RemoveClaimAsync(null, new Claim("Bamberbia", "Kergudu"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreRemoveClaimNullClaimThrowsExc()
        {
            var result = this.store.RemoveClaimAsync(new ApplicationUser(), null);
        }

        [TestMethod]
        public void IdentStoreRemoveClaimCallsDatabase()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var existingClaim = new Claim("FavoriteActress", "Sandra Bullock");
            List<IdentityUserClaim> claimList = new List<IdentityUserClaim>();
            claimList.AddRange(this.fakeUserFromDb.Claims);
            this.dbHelperMock.Setup(mock => mock.GetMany<IdentityUserClaim>(It.IsAny<Expression<Func<IdentityUserClaim, bool>>>()))
                .Returns<Expression<Func<IdentityUserClaim, bool>>>(predicate => claimList.Where<IdentityUserClaim>(predicate.Compile()).AsQueryable());
            var result = this.store.RemoveClaimAsync(appUser, existingClaim);
            this.dbHelperMock.Verify(m => m.GetMany(It.IsAny<Expression<Func<IdentityUserClaim, bool>>>()), Times.Once());
            this.dbHelperMock.Verify(m => m.Delete(It.IsAny<IdentityUserClaim>()), Times.Once());
            // this.fakeUserFromDb.Claims.Count.Should().Be(0);
        }

        [TestMethod]
        public void IdentStoreRemoveWrongClaimLeavesAppUserIntact()
        {
            var appUser = IdentityMapper.MapAppUserFromDbUser(this.fakeUserFromDb);
            var result = this.store.RemoveClaimAsync(appUser, new Claim("Facebook", RandomData.GetStringNumber(12)));
            appUser.Claims.Count.Should().Be(1);
        }

        [TestMethod]
        public void IdentStoreRemoveClaimRemovesInMemObject()
        {
            var appUser = IdentityMapper.MapAppUserFromDbUser(this.fakeUserFromDb);
            var existingClaim = new Claim("FavoriteActress", "Sandra Bullock");
            var result = this.store.RemoveClaimAsync(appUser, existingClaim);
            appUser.Claims.Count.Should().Be(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreAddToRoleNullUserThrowsExc()
        {
            var result = this.store.AddToRoleAsync(null, "Superuser");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IdentStoreAddToRoleNullRoleThrowsExc()
        {
            var result = this.store.AddToRoleAsync(new ApplicationUser(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IdentStoreAddToRoleNonExistingRoleThrowsExc()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            var result = this.store.AddToRoleAsync(appUser, "Feurer");
        }

        [TestMethod]
        public void IdentStoreAddToRoleCallsDatabase()
        {
            var appUser = new ApplicationUser { Id = this.fakeUserFromDb.Id, UserName = this.fakeUserFromDb.EmailAddress };
            List<IdentityUserRole> roleList = new List<IdentityUserRole>();
            roleList.AddRange(this.fakeUserFromDb.Roles);
            string roleName = RandomData.GetString();
            roleList.Add(new IdentityUserRole { RoleId = RandomData.GetInteger(1000, 9999), RoleName = roleName, Users = new List<IdentityUser>() });
            this.dbHelperMock.Setup(mock => mock.Get<IdentityUserRole>(It.IsAny<Expression<Func<IdentityUserRole, bool>>>()))
                .Returns<Expression<Func<IdentityUserRole, bool>>>(predicate => roleList.FirstOrDefault(predicate.Compile()));
            var result = this.store.AddToRoleAsync(appUser, roleName);

            // asserts
            this.dbHelperMock.Verify(m => m.Get<IdentityUserRole>(It.IsAny<Expression<Func<IdentityUserRole, bool>>>()), Times.Once());
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Once());
            this.fakeUserFromDb.Roles.Count.Should().Be(2); // One existing and one added
            appUser.Roles.Count.Should().Be(1); // We have no roles in this test sertup
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreRemoveFromRoleNullUserThrowsExc()
        {
            var result = this.store.AddToRoleAsync(null, "Superuser");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IdentStoreRemoveFromRoleEmptyRoleThrowsExc()
        {
            var result = this.store.AddToRoleAsync(new ApplicationUser(), string.Empty);
        }

        [TestMethod]
        public void IdentStoreRemoveFromRoleNonExistingRoleDoesNothing()
        {
            var appUser = new ApplicationUser
                              {
                                  Id = this.fakeUserFromDb.Id,
                                  UserName = this.fakeUserFromDb.EmailAddress,
                                  Roles = this.fakeUserFromDb.Roles.Select(r => new ApplicationRole { Id = r.RoleId, Name = r.RoleName }).ToList()
                              };
            var result = this.store.RemoveFromRoleAsync(appUser, "Feurer");

            appUser.Roles.Count.Should().Be(1);
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Never());
        }

        [TestMethod]
        public void IdentStoreRemoveFromRoleCallsDatabase()
        {
            var appUser = new ApplicationUser
                              {
                                  Id = this.fakeUserFromDb.Id,
                                  UserName = this.fakeUserFromDb.EmailAddress,
                                  Roles = this.fakeUserFromDb.Roles.Select(r => new ApplicationRole { Id = r.RoleId, Name = r.RoleName }).ToList()
                              };
            List<IdentityUserRole> roleList = new List<IdentityUserRole>();
            roleList.AddRange(this.fakeUserFromDb.Roles);
            this.dbHelperMock.Setup(mock => mock.Get<IdentityUserRole>(It.IsAny<Expression<Func<IdentityUserRole, bool>>>()))
                .Returns<Expression<Func<IdentityUserRole, bool>>>(predicate => roleList.FirstOrDefault(predicate.Compile()));
            var result = this.store.RemoveFromRoleAsync(appUser, appUser.Roles.First().Name);
            appUser.Roles.Count.Should().Be(0);
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreGetRolesNullUserThrowsExc()
        {
            var result = this.store.GetRolesAsync(null);
        }

        [TestMethod]
        public void IdentStoreGetRolesReturnsRoles()
        {
            var appUser = new ApplicationUser
            {
                Id = this.fakeUserFromDb.Id,
                UserName = this.fakeUserFromDb.EmailAddress,
                Roles = this.fakeUserFromDb.Roles.Select(r => new ApplicationRole { Id = r.RoleId, Name = r.RoleName }).ToList()
            };
            var result = this.store.GetRolesAsync(appUser);
            appUser.Roles.Count.Should().Be(1);
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreIsInRoleNullUserThrowsExc()
        {
            var result = this.store.IsInRoleAsync(null, "Superuser");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IdentStoreIsInRoleEmptyRoleThrowsExc()
        {
            var result = this.store.IsInRoleAsync(new ApplicationUser(), string.Empty);
        }

        [TestMethod]
        public void IdentStoreIsInExistingRoleReturnsOk()
        {
            var appUser = new ApplicationUser
            {
                Id = this.fakeUserFromDb.Id,
                UserName = this.fakeUserFromDb.EmailAddress,
                Roles = this.fakeUserFromDb.Roles.Select(r => new ApplicationRole { Id = r.RoleId, Name = r.RoleName }).ToList()
            };
            var result = this.store.IsInRoleAsync(appUser, this.fakeUserFromDb.Roles.First().RoleName);
            result.Result.Should().BeTrue();
            this.dbHelperMock.Verify(m => m.GetById<IdentityUser, Guid>(appUser.Id), Times.Never());
        }

        [TestMethod]
        public void IdentStoreIsInNonExistingRoleReturnsFalse()
        {
            var appUser = new ApplicationUser
            {
                Id = this.fakeUserFromDb.Id,
                UserName = this.fakeUserFromDb.EmailAddress,
                Roles = this.fakeUserFromDb.Roles.Select(r => new ApplicationRole { Id = r.RoleId, Name = r.RoleName }).ToList()
            };
            var result = this.store.IsInRoleAsync(appUser, "IDoNotExist");
            result.Result.Should().BeFalse();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreSetStampNullUserThrowsExc()
        {
            var result = this.store.SetSecurityStampAsync(null, "stampy");
        }

        [TestMethod]
        public void IdentStoreSetStampChangesUser()
        {
            var appUser = new ApplicationUser
            {
                Id = this.fakeUserFromDb.Id,
                UserName = this.fakeUserFromDb.EmailAddress,
                SecurityStamp = "Initial"
            };
            string stamp = RandomData.GetStringWord();
            var result = this.store.SetSecurityStampAsync(appUser, stamp);
            appUser.SecurityStamp.Should().Be(stamp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IdentStoreGetStampNullUserThrowsExc()
        {
            var result = this.store.GetSecurityStampAsync(null);
        }

        [TestMethod]
        public void IdentStoreGetStampFromUser()
        {
            string stamp = RandomData.GetStringWord();
            var appUser = new ApplicationUser
            {
                Id = this.fakeUserFromDb.Id,
                UserName = this.fakeUserFromDb.EmailAddress,
                SecurityStamp = stamp
            };
            var result = this.store.GetSecurityStampAsync(appUser);
            result.Result.Should().Be(stamp);
        }
    }
}
