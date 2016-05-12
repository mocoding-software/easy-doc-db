using System;
using Xunit;
using Mocoding.EasyDocDb.Yaml;

namespace Mocoding.EasyDocDb.Tests.Yaml
{
    public class YamlTests
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
            FullName = "Name",
        };

        private YamlSerializer _serializer = new YamlSerializer();

        [Fact]
        public void SerializeDeserializeTest()
        {
            var yaml = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(yaml);

            Assert.Equal(_person.Salary, obj.Salary);
            Assert.Equal(_person.DateOfBirth, obj.DateOfBirth);
            Assert.Equal(_person.FullName, obj.FullName);
            Assert.Equal(_person.Address.Street, obj.Address.Street);
            Assert.Equal(_person.Address.City, obj.Address.City);
        }
    }
}
