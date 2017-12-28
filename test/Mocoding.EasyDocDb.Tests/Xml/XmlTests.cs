using System;
using Mocoding.EasyDocDb.Xml;
using NUnit.Framework;

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

        [Test(Description = "Serialize Deserialize Test")]
        public void SerializeDeserializeTest()
        {
            var xml = _serializer.Serialize(_person);
            var obj = _serializer.Deserialize<Person>(xml);

            Assert.AreEqual(_person.Salary, obj.Salary);
            Assert.AreEqual(_person.DateOfBirth, obj.DateOfBirth);
            Assert.AreEqual(_person.FullName, obj.FullName);
            Assert.AreEqual(_person.Address.Street, obj.Address.Street);
            Assert.AreEqual(_person.Address.City, obj.Address.City);
        }
    }
}
