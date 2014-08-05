namespace Uma.Eservices.LogicTests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class IdentityMapperTests
    {
        [TestMethod]
        public void IdentityMapperAppUserIsCreatedIfNotGiven()
        {
            IdentityUser dbUser = new IdentityUser();
            var result = IdentityMapper.MapAppUserFromDbUser(dbUser);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void IdentityMapperAppUserIsNullIfDbUserIsNull()
        {
            var result = IdentityMapper.MapAppUserFromDbUser(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void IdentityMapperAppUserMainPropertiesAreMapped()
        {
            IdentityUser dbUser = new IdentityUser
                                      {
                                          EmailAddress = RandomData.GetEmailAddress(),
                                          Id = Guid.NewGuid()
                                      };
            ApplicationUser appUser = new ApplicationUser
                                          {
                                              UserName = RandomData.GetEmailAddress(),
                                              Id = Guid.NewGuid()
                                          };
            ApplicationUser result = IdentityMapper.MapAppUserFromDbUser(dbUser, appUser);
            result.Should().NotBeNull();
            result.Id.Should().Be(dbUser.Id);
            result.UserName.Should().Be(dbUser.EmailAddress);
        }

        [TestMethod]
        public void IdentityMapperAppUserRelationsAreMapped()
        {
            IdentityUser dbUser = new IdentityUser
                                      {
                                          Logins =
                                              new List<IdentityUserLogin>
                                                  {
                                                      new IdentityUserLogin(),
                                                      new IdentityUserLogin(),
                                                      new IdentityUserLogin()
                                                  },
                                          Roles = new List<IdentityUserRole>
                                                      {
                                                          new IdentityUserRole(),
                                                          new IdentityUserRole()
                                                      },
                                          Claims = new List<IdentityUserClaim>
                                                       {
                                                           new IdentityUserClaim()
                                                       }
                                      };

            ApplicationUser result = IdentityMapper.MapAppUserFromDbUser(dbUser);
            result.Should().NotBeNull();
            result.Logins.Should().NotBeNull();
            result.Logins.Count.Should().Be(3);
            result.Roles.Should().NotBeNull();
            result.Roles.Count.Should().Be(2);
            result.Claims.Should().NotBeNull();
            result.Claims.Count.Should().Be(1);
        }

        [TestMethod]
        public void IdentityMapperDbUserIsCreatedIfNotGiven()
        {
            ApplicationUser appUser = new ApplicationUser();
            var result = IdentityMapper.MapDbUserFromAppUser(appUser);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void IdentityMapperDbUserIsNullIfAppUserIsNull()
        {
            var result = IdentityMapper.MapDbUserFromAppUser(null);
            result.Should().BeNull();
        }

        [TestMethod]
        public void IdentityMapperDbUserMainPropertiesAreMapped()
        {
            IdentityUser dbUser = new IdentityUser
            {
                EmailAddress = RandomData.GetEmailAddress(),
                Id = Guid.NewGuid()
            };
            ApplicationUser appUser = new ApplicationUser
            {
                UserName = RandomData.GetEmailAddress(),
                Id = Guid.NewGuid()
            };
            var result = IdentityMapper.MapDbUserFromAppUser(appUser, dbUser);
            result.Should().NotBeNull();
            result.Id.Should().Be(appUser.Id);
            result.EmailAddress.Should().Be(appUser.UserName);
        }
    }
}
