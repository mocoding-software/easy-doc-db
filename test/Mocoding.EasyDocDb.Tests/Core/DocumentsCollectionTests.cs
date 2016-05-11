using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using NSubstitute;
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
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer);

            // Act
            var actual = collection.New();

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void AllEmptyTest()
        {
            // Arrange
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer);

            // Act
            var actual = collection.All();

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async void AllTest()
        {
            // Arrange
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();
            var collection = new DocumentsCollection<Person>(REF, storage, serializer);
            var people = new string[3] { "Kirill", "Dima", "Kate" };

            // Act
            storage.Enumerate(REF).Returns(people);
            await collection.Init();

            var actual = collection.All();

            // Assert
            Assert.Equal(people.Count(), actual.Count());
        }

    }
}
