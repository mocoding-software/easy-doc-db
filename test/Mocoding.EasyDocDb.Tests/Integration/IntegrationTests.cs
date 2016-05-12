using System;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using Mocoding.EasyDocDb.Json;
using NSubstitute;
using Xunit;
using System.Threading;
using System.IO;
using System.Linq;
using Mocoding.EasyDocDb.Tests.Helpers;

namespace Mocoding.EasyDocDb.Tests.IntegrationTests
{
    public class IntegrationTests
    {
        const string REF = "test_data";

        public IntegrationTests()
        {
            TestDir.EnsureCreated(REF);
        }

        private Person _person = new Person()
        {
            Address = new Address()
            {
                Street = "Unknown",
                City = "Newermind"
            },
            Salary = 100,
            DateOfBirth = new DateTime(),
            FullName = "Name1"
        };

        [Fact]
        public async Task SaveDocumentTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var serializer = new JsonSerializer();
            var @ref = Path.Combine(REF, "saveTest");

            // Act
            var docCollection = new DocumentsCollection<Person>(@ref, storage, serializer);
            await docCollection.Init();

            var expectedDoc = docCollection.New();
            expectedDoc.Data.Salary = _person.Salary;
            expectedDoc.Data.FullName = _person.FullName;
            await expectedDoc.Save();

            // exists in docCollection
            var actualFromColl = docCollection.All()
                .Where(_ => _.Data.FullName == _person.FullName && _.Data.Salary == _person.Salary).FirstOrDefault();

            // exists on disk
            DirectoryInfo dirInfo = new DirectoryInfo(@ref);
            var actualFile = dirInfo.GetFiles("*.json").OrderBy(_ => _.CreationTime).LastOrDefault();
            var actualPerson = File.ReadAllText(actualFile.FullName);
            var actualFromDisk = serializer.Deserialize<Person>(actualPerson);

            // Assert
            Assert.Equal(expectedDoc.Data.Salary, actualFromColl.Data.Salary);
            Assert.Equal(expectedDoc.Data.FullName, actualFromColl.Data.FullName);

            Assert.Equal(expectedDoc.Data.Salary, actualFromDisk.Salary);
            Assert.Equal(expectedDoc.Data.FullName, actualFromDisk.FullName);

        }

    }
}
