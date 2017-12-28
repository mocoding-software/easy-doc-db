using System;
using Mocoding.EasyDocDb.Yaml;
using NUnit.Framework;

namespace Mocoding.EasyDocDb.Tests.Yaml
{
    public class YamlTests
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
            FullName = "Name",
        };

        private YamlSerializer _serializer = new YamlSerializer();

        [Test(Description = "Serialize Deserialize Test")]
        public void SerializeDeserializeTest()
        {
            var yaml = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(yaml);

            Assert.AreEqual(_person.Salary, obj.Salary);
            Assert.AreEqual(_person.DateOfBirth, obj.DateOfBirth);
            Assert.AreEqual(_person.FullName, obj.FullName);
            Assert.AreEqual(_person.Address.Street, obj.Address.Street);
            Assert.AreEqual(_person.Address.City, obj.Address.City);
        }
    }
}
