using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.FileSystem;
using Mocoding.EasyDocDb.Json;
using Mocoding.EasyDocDb.Tests.Helpers;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Integration
{
    public class IntegrationTests
    {
        private const string REF = "test_data";
        private readonly Person _person;
        public IntegrationTests()
        {
            TestDir.EnsureCreated(REF);
            _person = new Person()
            {
                Address = new Address()
                {
                    Street = "Unknown",
                    City = "Newermind"
                },
                Salary = 100,
                DateOfBirth = default(DateTime),
                FullName = "Name1"
            };
        }

        [Fact(DisplayName = "Save Document Test")]
        public async Task SaveDocumentTest()
        {
            var serializer = new JsonSerializer();
            var @ref = Path.Combine(REF, "saveTest");
            IRepository repo = new Repository(serializer, new EmbeddedStorage());
            var docCollection = await repo.InitCollection<Person>(@ref);

            var expectedDoc = docCollection.New();
            expectedDoc.Data.Salary = _person.Salary;
            expectedDoc.Data.FullName = _person.FullName;
            await expectedDoc.Save();

            // exists in docCollection
            var actualFromColl = docCollection.Documents
                .Where(_ => _.Data.FullName == expectedDoc.Data.FullName && _.Data.Salary == expectedDoc.Data.Salary)
                .FirstOrDefault();

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

        [Fact(DisplayName = "Delete Document Test")]
        public async Task DeleteDocumentTest()
        {
            var serializer = new JsonSerializer();
            var @ref = Path.Combine(REF, "deleteTest");
            IRepository repo = new Repository(serializer, new EmbeddedStorage());
            var docCollection = await repo.InitCollection<Person>(@ref);

            var expectedDoc = docCollection.New();
            expectedDoc.Data.Salary = _person.Salary;
            expectedDoc.Data.FullName = _person.FullName;
            await expectedDoc.Save();

            await expectedDoc.Delete();

            var actualFromColl = docCollection.Documents
                .FirstOrDefault(_ => _.Data.FullName == expectedDoc.Data.FullName && _.Data.Salary == expectedDoc.Data.Salary);

            DirectoryInfo dirInfo = new DirectoryInfo(@ref);
            var actualFile = dirInfo.GetFiles("*.json").OrderBy(_ => _.CreationTime).LastOrDefault();

            // Assert
            Assert.Null(actualFromColl);
            Assert.Null(actualFile);
        }

        [Fact(DisplayName = "Update Document Test")]
        public async Task UpdateDocumentTest()
        {
            var serializer = new JsonSerializer();
            var @ref = Path.Combine(REF, "updateTest");
            IRepository repo = new Repository(serializer, new EmbeddedStorage());
            var docCollection = await repo.InitCollection<Person>(@ref);
            var newName = Guid.NewGuid().ToString();

            var expectedDoc = docCollection.New();
            expectedDoc.Data.Salary = _person.Salary;
            expectedDoc.Data.FullName = _person.FullName;
            await expectedDoc.Save();
            await expectedDoc.SyncUpdate(_ => _.FullName = newName);

            // exists in docCollection
            var actualFromColl = docCollection.Documents
                .Where(_ => _.Data.FullName == expectedDoc.Data.FullName && _.Data.Salary == expectedDoc.Data.Salary)
                .FirstOrDefault();

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

        [Fact(DisplayName = "Update Document Test2")]
        public async Task UpdateDocumentTest2()
        {
            var serializer = new JsonSerializer();
            var @ref = Path.Combine(REF, "updateTest2");
            IRepository repo = new Repository(serializer, new EmbeddedStorage());
            var docCollection = await repo.InitCollection<Person>(@ref);
            var newName = Guid.NewGuid().ToString();

            var doc = docCollection.New(_person);
            await doc.Save();

            Assert.Equal(doc.Data.Salary, _person.Salary);
            Assert.Equal(doc.Data.FullName, _person.FullName);

            var newPerson = new Person { FullName = newName };
            await doc.SyncUpdate(newPerson);

            // Assert
            Assert.Equal(doc.Data.Salary, 0);
            Assert.Equal(doc.Data.FullName, newName);
        }
    }
}
