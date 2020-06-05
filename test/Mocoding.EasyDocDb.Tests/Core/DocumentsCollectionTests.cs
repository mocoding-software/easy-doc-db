using System.Linq;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using NSubstitute;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class DocumentsCollectionTests
    {
        private const string REF = "test_ref";

        [Fact(DisplayName = "new test")]
        public void NewTest()
        {
            // Arrange
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer);

            // Act
            var actual = collection.New();

            // Assert
            Assert.NotNull(actual);
        }

        [Fact(DisplayName = "All Empty Test")]
        public void AllEmptyTest()
        {
            // Arrange
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer);

            // Act
            var actual = collection.Documents;

            // Assert
            Assert.Empty(actual);
        }

        [Fact(DisplayName = "AllTest")]
        public async Task AllTest()
        {
            // Arrange
            var storage = Substitute.For<IDocumentStorage>();
            var people = new[] { "Kirill", "Dima", "Kate" };
            storage.Enumerate(REF).Returns(Task.FromResult(people));
            var serializer = Substitute.For<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer);

            // Act
            await collection.Init();

            var actual = collection;

            // Assert
            Assert.Equal(people.Count(), actual.Documents.Length);
        }
    }
}
