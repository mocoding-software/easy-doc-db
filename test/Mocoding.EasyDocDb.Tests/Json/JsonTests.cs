using System;
using Xunit;
using Mocoding.EasyDocDb.Json;

namespace Mocoding.EasyDocDb.Tests.Json
{
    public class JsonTests
    {
        private Person _person = new Person()
        {
            Address = new Address()
            {
                Street = "Unknown",
                City = "Newermind"
            },
            Salary = 100,
            DateOfBirth = new DateTime(),
            FullName = "Name"
        };

        private JsonSerializer _serializer = new JsonSerializer();

        [Fact]
        public void SerializeDeserializeTest()
        {
            var json = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(json);

            Assert.Equal(_person.Salary, obj.Salary);
            Assert.Equal(_person.DateOfBirth, obj.DateOfBirth);
            Assert.Equal(_person.FullName, obj.FullName);
            Assert.Equal(_person.Address.Street, obj.Address.Street);
            Assert.Equal(_person.Address.City, obj.Address.City);
        }
    }
}
