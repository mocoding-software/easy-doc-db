using System.IO;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using Mocoding.EasyDocDb.FileSystem;
using Mocoding.EasyDocDb.Tests.Helpers;
using Xunit;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class EmbeddedStorageTests
    {
        private const string Ref = "test_data";

        public EmbeddedStorageTests()
        {
            TestDir.EnsureCreated(Ref);
        }

        [Fact(DisplayName = "New Ref Test")]
        public void NewRefTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            const string fileName = "fileName";

            // Act
            var actual = storage.NewRef(Ref, fileName);

            // Assert
            Assert.Equal(Path.Combine(Ref, fileName), actual);
        }

        [Fact(DisplayName = "Write Test")]
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
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Read Test")]
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
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Delete Test")]
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

        [Fact(DisplayName = "Enumerate Empty Test")]
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
            Assert.Empty(actual);
        }

        [Fact(DisplayName = "Enumerate Test")]
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
            Assert.NotEmpty(actual);
            Assert.Equal(data[0], tomato);
        }
    }
}
