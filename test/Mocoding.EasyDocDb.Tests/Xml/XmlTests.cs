using System;
using Mocoding.EasyDocDb.Xml;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Xml
{
    public class XmlTests
    {
        private readonly Person _person = new Person()
        {
            Address = new Address()
            {
                Street = "Unknown",
                City = "Newermind"
            },
            Salary = 100,
            DateOfBirth = default(DateTime),
            FullName = "Name"
        };

        private readonly XmlSerializer _serializer = new XmlSerializer();

        [Fact]
        public void SerializeDeserializeTest()
        {
            var xml = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(xml);

            Assert.Equal(_person.Salary, obj.Salary);
            Assert.Equal(_person.DateOfBirth, obj.DateOfBirth);
            Assert.Equal(_person.FullName, obj.FullName);
            Assert.Equal(_person.Address.Street, obj.Address.Street);
            Assert.Equal(_person.Address.City, obj.Address.City);
        }
    }
}
