using System;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using NSubstitute;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class DocumentTests
    {
        const string REF = "test_ref";

        [Fact]
        public void NewDocumentTest()
        {
            var document = new Document<Person>(REF, null, null);

            Assert.NotNull(document.Data);
        }

        [Fact]
        public async Task SaveDocumentTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var document = new Document<Person>(REF, storage, serializer);
            var expectedContent = "test content";
            serializer.Serialize(document.Data).Returns(expectedContent);

            await document.Save();

            serializer.Received().Serialize(document.Data);
            await storage.Received().Write(REF, expectedContent);
        }

        [Fact]
        public async Task LoadDocumentTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var expectedContent = "test content";
            var expectedName = "test name";
            storage.Read(REF).Returns(Task.FromResult(expectedContent));
            serializer.Deserialize<Person>(expectedContent).Returns(new Person() {FullName = expectedName});

            var document = new Document<Person>(REF, storage, serializer);
            await document.Init();

            Assert.Equal(expectedName, document.Data.FullName);
        }

        [Fact]
        public async Task DeleteCallbackTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var callbackCalled =  false;
            var document = new Document<Person>(REF, storage, serializer, d =>
            {
                Assert.NotNull(d);
                callbackCalled = true;

            });

            await document.Delete();

            Assert.True(callbackCalled);
            await storage.Received().Delete(REF);


            callbackCalled = false;

            await document.Delete();

            Assert.False(callbackCalled);
        }

        [Fact]
        public async Task SaveCallbackTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var callbackCalled = false;
            var document = new Document<Person>(REF, storage, serializer, null, d =>
            {
                Assert.NotNull(d);
                callbackCalled = true;

            });

            await document.Save();

            Assert.True(callbackCalled);

            callbackCalled = false;

            await document.Save();

            Assert.False(callbackCalled);
        }
    }
}
