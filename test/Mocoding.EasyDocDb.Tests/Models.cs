using System;

namespace Mocoding.EasyDocDb.Tests
{
    public class Person
    {
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }
    }
}
