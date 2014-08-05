namespace Uma.Eservices.CommonTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Common.Extenders;
    using Uma.Eservices.TestHelpers;
    using FluentAssertions;

    /// <summary>
    /// Class validates Linq Join custome methods.
    /// Test data are the same like:
    /// http://www.codeproject.com/Articles/488643/LinQ-Extended-Joins
    /// </summary>
    [TestClass]
    public class JoinMethodTests
    {
        private List<Person> testPersons;

        private List<Address> testAddresses;

        private const int PersonCount = 9;

        private const int AddressCount = 5;

        private const string NullValue = "Null value";

        [TestInitialize]
        public void TestSetup()
        {
            this.testPersons = new List<Person>();
            this.testAddresses = new List<Address>();

            for (int i = 0; i < PersonCount; i++)
            {
                this.testPersons.Add(new Person
                {
                    ID = RandomData.GetStringWordProper(),
                    Age = RandomData.GetInteger(i, 50),
                    Name = RandomData.GetStringWordProper(),
                    IdAddress = i + 1
                });
            }

            for (int i = 0; i < AddressCount; i++)
            {
                this.testAddresses.Add(new Address
                {
                    City = RandomData.GetStringWordProper(),
                    Street = RandomData.GetStringWordProper(),
                });
            }

            this.testAddresses[0].IdAddress = 1;
            this.testAddresses[1].IdAddress = 2;
            this.testAddresses[2].IdAddress = 3;
            this.testAddresses[3].IdAddress = 4;
            this.testAddresses[4].IdAddress = 99;
        }

        [TestMethod]
        public void LeftJoinSet()
        {
            var res = this.testPersons.LeftJoin(this.testAddresses, p => p.IdAddress, a => a.IdAddress,
                                                                (p, a) => new { MyPerson = p, MyAddress = a }).
                                                                Select(a => new
                                                                {
                                                                    Street = (a.MyAddress != null ? a.MyAddress.Street : NullValue),
                                                                    Persons = (a.MyPerson != null ? a.MyPerson.Name : NullValue),
                                                                });

            res.Where(o => o.Street == NullValue).Count().Should().Be(5);
            res.Where(o => !string.IsNullOrEmpty(o.Persons)).Count().Should().Be(9);
        }

        [TestMethod]
        public void RightJoinSet()
        {
            var res = this.testPersons.RightJoin(this.testAddresses, p => p.IdAddress, a => a.IdAddress,
                                                                (p, a) => new { MyPerson = p, MyAddress = a }).
                                                                Select(a => new
                                                                {
                                                                    Street = (a.MyAddress != null ? a.MyAddress.Street : NullValue),
                                                                    Persons = (a.MyPerson != null ? a.MyPerson.Name : NullValue),
                                                                });

            res.Where(o => !string.IsNullOrEmpty(o.Street)).Count().Should().Be(5);
            res.Where(o => o.Persons == NullValue).Count().Should().Be(1);
        }

        [TestMethod]
        public void FullOuterJoinSet()
        {
            var res = this.testPersons.FullOuterJoin(this.testAddresses, p => p.IdAddress, a => a.IdAddress,
                                                                (p, a) => new { MyPerson = p, MyAddress = a }).
                                                                Select(a => new
                                                                {
                                                                    Street = (a.MyAddress != null ? a.MyAddress.Street : NullValue),
                                                                    Persons = (a.MyPerson != null ? a.MyPerson.Name : NullValue),
                                                                });

            res.Where(o => o.Street == NullValue).Count().Should().Be(5);
            res.Where(o => !string.IsNullOrEmpty(o.Street) & o.Street != NullValue).Count().Should().Be(5);

            res.Where(o => o.Persons == NullValue).Count().Should().Be(1);
            res.Where(o => !string.IsNullOrEmpty(o.Persons) & o.Persons != NullValue).Count().Should().Be(9);
        }

        [TestMethod]
        public void LeftExludingJoin()
        {
            var res = this.testPersons.LeftExcludingJoin(this.testAddresses, p => p.IdAddress, a => a.IdAddress,
                                                                (p, a) => new { MyPerson = p, MyAddress = a }).
                                                                Select(a => new
                                                                {
                                                                    Street = (a.MyAddress != null ? a.MyAddress.Street : NullValue),
                                                                    Persons = (a.MyPerson != null ? a.MyPerson.Name : NullValue),
                                                                });

            res.Where(o => o.Street == NullValue).Count().Should().Be(5);
            res.Where(o => !string.IsNullOrEmpty(o.Persons)).Count().Should().Be(5);
        }

        [TestMethod]
        public void RightExludingJoinSet()
        {
            var res = this.testPersons.RightExcludingJoin(this.testAddresses, p => p.IdAddress, a => a.IdAddress,
                                                                (p, a) => new { MyPerson = p, MyAddress = a }).
                                                                Select(a => new
                                                                {
                                                                    Street = (a.MyAddress != null ? a.MyAddress.Street : NullValue),
                                                                    Persons = (a.MyPerson != null ? a.MyPerson.Name : NullValue),
                                                                });

            res.Where(o => o.Persons == NullValue).Count().Should().Be(1);
            res.Where(o => !string.IsNullOrEmpty(o.Street)).Count().Should().Be(1);
        }

        [TestMethod]
        public void FullOuterExludingJoinSet()
        {
            var res = this.testPersons.FulltExcludingJoin(this.testAddresses, p => p.IdAddress, a => a.IdAddress,
                                                                (p, a) => new { MyPerson = p, MyAddress = a }).
                                                                Select(a => new
                                                                {
                                                                    Street = (a.MyAddress != null ? a.MyAddress.Street : NullValue),
                                                                    Persons = (a.MyPerson != null ? a.MyPerson.Name : NullValue),
                                                                });

            res.Where(o => o.Street == NullValue).Count().Should().Be(5);
            res.Where(o => o.Persons == NullValue).Count().Should().Be(1);

            res.Where(o => !string.IsNullOrEmpty(o.Persons) & o.Persons != NullValue).Count().Should().Be(5);
            res.Where(o => !string.IsNullOrEmpty(o.Street) & o.Street != NullValue).Count().Should().Be(1);
        }

    }
}
