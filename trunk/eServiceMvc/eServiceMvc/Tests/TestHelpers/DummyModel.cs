namespace Uma.Eservices.TestHelpers
{
    using System.ComponentModel.DataAnnotations;

    public class Person
    {
        [Required]
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int IdAddress { get; set; }
    }

    public class Address
    {
        public int IdAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
}
