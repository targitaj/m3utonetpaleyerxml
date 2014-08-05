namespace Uma.Eservices.ModelTests.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Models.Account;

    [TestClass]
    public class IdentityClassTests
    {
        [TestMethod]
        public void ApplicationUserCollectionsAreNotNulls()
        {
            WebUser user = new WebUser();

            // Check all collections (claims, roles) - they must be initialized as empty, but not nulls
            foreach (PropertyInfo objList in typeof(WebUser).GetProperties().Where(pt => pt.PropertyType.IsGenericType))
            {
                if (objList.PropertyType.Name.StartsWith("ICollection"))
                {
                    objList.GetValue(user, null).Should().NotBeNull();
                }
            }
        }
    }
}
