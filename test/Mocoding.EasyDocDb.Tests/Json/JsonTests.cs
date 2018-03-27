using System;
using Mocoding.EasyDocDb.Json;
using NUnit.Framework;

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
            DateOfBirth = default(DateTime),
            FullName = "Name"
        };

        private JsonSerializer _serializer = new JsonSerializer();

        [Test(Description = "Serialize Deserialize Test")]
        public void SerializeDeserializeTest()
        {
            var json = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(json);

            Assert.AreEqual(_person.Salary, obj.Salary);
            Assert.AreEqual(_person.DateOfBirth, obj.DateOfBirth);
            Assert.AreEqual(_person.FullName, obj.FullName);
            Assert.AreEqual(_person.Address.Street, obj.Address.Street);
            Assert.AreEqual(_person.Address.City, obj.Address.City);
        }

        [Test(Description = "Deserialize Empty Test")]
        public void DeserializeEmptyTest()
        {
            var obj = _serializer.Deserialize<Person>(string.Empty);
            Assert.AreEqual(null, obj);
        }
    }
}
