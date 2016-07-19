using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using Moq;
using Xunit;
using System.IO;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class DocumentsCollectionTests
    {
        const string REF = "test_ref";

        [Fact]
        public void NewTest()
        {
            // Arrange
            var storage = new Mock<IDocumentStorage>();
            var serializer = new Mock<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage.Object, serializer.Object);

            // Act
            var actual = collection.New();

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void AllEmptyTest()
        {
            // Arrange
            var storage = Mock.Of<IDocumentStorage>();
            var serializer = new Mock<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer.Object);

            // Act
            var actual = collection;

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async void AllTest()
        {
            // Arrange
            var storage = new Mock<IDocumentStorage>();
            var serializer = new Mock<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage.Object, serializer.Object);
            var people = new string[3] { "Kirill", "Dima", "Kate" };

            // Act
            storage.Setup(i => i.Enumerate(REF)).Returns(Task.FromResult(people));
            await collection.Init();

            var actual = collection;

            // Assert
            Assert.Equal(people.Count(), actual.Count);
        }

    }
}
