using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;
using NSubstitute;
using Xunit;
using System.IO;
using Mocoding.EasyDocDb.Tests.Helpers;

namespace Mocoding.EasyDocDb.Tests.Core
{
    public class EmbeddedStorageTests
    {
        const string REF = "test_data";

        public EmbeddedStorageTests()
        {
            TestDir.EnsureCreated(REF);
        }

        [Fact]
        public void NewRefTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = "fileName";

            // Act
            var actual = storage.NewRef(REF, fileName);

            // Assert
            Assert.Equal(Path.Combine(REF, fileName), actual);
        }

        [Fact]
        public async Task WriteTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(REF, "write");
            var expected = "document content";

            // Act
            await storage.Write(fileName, expected);
            var actual = File.ReadAllText(fileName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ReadTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(REF, "read");
            var expected = "document content";

            // Act
            File.WriteAllText(fileName, expected);
            var actual = await storage.Read(fileName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(REF, "delete");

            // Act
            File.WriteAllText(fileName, "delete Test");
            await storage.Delete(fileName);

            // Assert
            Assert.False(File.Exists(fileName));
        }

        [Fact]
        public async Task EnumerateEmptyTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var folder = Path.Combine(REF, "_empty");

            // Act
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            var actual = await storage.Enumerate(folder);

            // Assert
            Assert.True(Directory.Exists(folder));
            Assert.Empty(actual);
        }

        [Fact]
        public async Task EnumerateTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var data = new string[3] { "Tomato", "Banana", "Plum" };
            var dir = Path.Combine(REF, "_test");

            // Act
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

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
