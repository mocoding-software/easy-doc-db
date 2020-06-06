using System;
using System.Threading;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using NSubstitute;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class DocumentTests
    {
        private const string Ref = "test_ref";

        [Fact(DisplayName = "New Document Test")]
        public void NewDocumentTest()
        {
            var document = new Document<Person>(Ref, null, null);

            Assert.NotNull(document.Data);
        }

        [Fact(DisplayName = "Save Document Test")]
        public async Task SaveDocumentTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var document = new Document<Person>(Ref, storage, serializer);
            var expectedContent = "test content";
            serializer.Serialize(document.Data).Returns(expectedContent);

            await document.Save();

            serializer.Serialize(document.Data);
            await storage.Write(Ref, expectedContent);
        }

        [Fact(DisplayName = "Load Document Test")]
        public async Task LoadDocumentTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var expectedContent = "test content";
            var expectedName = "test name";
            storage.Read(Ref).Returns(Task.FromResult(expectedContent));
            serializer.Deserialize<Person>(expectedContent).Returns(new Person() { FullName = expectedName });

            var document = new Document<Person>(Ref, storage, serializer);
            await document.Init();

            var doc = document.Data;
            Assert.Equal(expectedName, doc.FullName);
        }

        [Fact(DisplayName = "Delete Callback Test")]
        public async Task DeleteCallbackTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var callbackCalled = false;
            var document = new Document<Person>(Ref, storage, serializer, d =>
            {
                Assert.NotNull(d);
                callbackCalled = true;
            });

            await document.Delete();

            Assert.True(callbackCalled);
            await storage.Delete(Ref);

            callbackCalled = false;

            await document.Delete();

            Assert.False(callbackCalled);
        }

        [Fact(DisplayName = "Save Callback Test")]
        public async Task SaveCallbackTest()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();

            var callbackCalled = false;
            var document = new Document<Person>(Ref, storage, serializer, null, d =>
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

        [Fact(DisplayName = "Check Exception")]
        public async void CheckException()
        {
            var storage = Substitute.For<IDocumentStorage>();
            var serializer = Substitute.For<IDocumentSerializer>();
            var document = new Document<Person>(Ref, storage, serializer);

            var task = Task.Run(() => document.SyncUpdate(_ => Thread.Sleep(10000)));

            var ex = await Assert.ThrowsAsync<EasyDocDbException>(() => Task.Run(() => document.Save()));

            Assert.Equal("Timeout! Can't get exclusive access to document.", ex.Message);
        }
    }
}
