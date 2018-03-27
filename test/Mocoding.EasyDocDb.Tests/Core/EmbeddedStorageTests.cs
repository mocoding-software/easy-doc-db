using System.IO;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using Mocoding.EasyDocDb.FileSystem;
using Mocoding.EasyDocDb.Tests.Helpers;
using NUnit.Framework;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class EmbeddedStorageTests
    {
        private const string Ref = "test_data";

        public EmbeddedStorageTests()
        {
            TestDir.EnsureCreated(Ref);
        }

        [Test(Description = "New Ref Test")]
        public void NewRefTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            const string fileName = "fileName";

            // Act
            var actual = storage.NewRef(Ref, fileName);

            // Assert
            Assert.AreEqual(Path.Combine(Ref, fileName), actual);
        }

        [Test(Description = "Write Test")]
        public async Task WriteTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(Ref, "write");
            var expected = "document content";

            // Act
            await storage.Write(fileName, expected);
            var actual = File.ReadAllText(fileName);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Read Test")]
        public async Task ReadTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(Ref, "read");
            var expected = "document content";

            // Act
            File.WriteAllText(fileName, expected);
            var actual = await storage.Read(fileName);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Delete Test")]
        public async Task DeleteTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(Ref, "delete");

            // Act
            File.WriteAllText(fileName, "delete Test");
            await storage.Delete(fileName);

            // Assert
            Assert.False(File.Exists(fileName));
        }

        [Test(Description = "Enumerate Empty Test")]
        public async Task EnumerateEmptyTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var folder = Path.Combine(Ref, "_empty");

            // Act
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            var actual = await storage.Enumerate(folder);

            // Assert
            Assert.True(Directory.Exists(folder));
            Assert.IsEmpty(actual);
        }

        [Test(Description = "Enumerate Test")]
        public async Task EnumerateTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var data = new[] { "Tomato", "Banana", "Plum" };
            var dir = Path.Combine(Ref, "_test");

            // Act
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            foreach (var item in data)
            {
                File.WriteAllText(Path.Combine(dir, item), item);
            }

            var actual = await storage.Enumerate(dir);
            var tomato = File.ReadAllText(Path.Combine(dir, data[0]));

            // Assert
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(data[0], tomato);
        }
    }
}
