using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Mocoding.EasyDocDb.Csv;
using Mocoding.EasyDocDb.Xml;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Csv
{
    public class CsvTests
    {
        private readonly Person _person = new Person()
        {
            Address = new Address()
            {
                Street = "Unknown",
                City = "Newermind",
            },
            Salary = 100,
            DateOfBirth = default(DateTime),
            FullName = "Name",
        };

        private readonly CsvSerializer _serializer = new CsvSerializer();

        [Fact(DisplayName = "Serialize Deserialize Test")]
        public void SerializeDeserializeTest()
        {
            var csv = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(csv);

            Assert.Equal(_person.Salary, obj.Salary);
            Assert.Equal(_person.DateOfBirth, obj.DateOfBirth);
            Assert.Equal(_person.FullName, obj.FullName);
            Assert.Equal(_person.Address.Street, obj.Address.Street);
            Assert.Equal(_person.Address.City, obj.Address.City);
        }

        [Fact(DisplayName = "Serialize Deserialize List Test")]
        public void SerializeDeserializeListTest()
        {
            var list = new List<Person>() { _person, _person, _person};
            var csv = _serializer.Serialize(list);
            var obj = _serializer.Deserialize<List<Person>>(csv);

            Assert.Equal(list.Count, obj.Count);
            Assert.All(obj, _ =>
            {
                Assert.Equal(_person.Salary, _.Salary);
                Assert.Equal(_person.DateOfBirth, _.DateOfBirth);
                Assert.Equal(_person.FullName, _.FullName);
                Assert.Equal(_person.Address.Street, _.Address.Street);
                Assert.Equal(_person.Address.City, _.Address.City);
            });
        }
    }
}
