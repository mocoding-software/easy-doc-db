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
    public class EmbeddedStorageTests
    {
        const string REF = "test_data";

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
        public async void WriteTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(REF, "write");
            var expected = "document content";

            // Act
            EnsureTestDirCreated();

            await storage.Write(fileName, expected);
            var actual = File.ReadAllText(fileName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ReadTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(REF, "read");
            var expected = "document content";

            // Act
            EnsureTestDirCreated();

            File.WriteAllText(fileName, expected);
            var actual = await storage.Read(fileName);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void DeleteTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var fileName = Path.Combine(REF, "delete");

            // Act
            EnsureTestDirCreated();

            File.WriteAllText(fileName, "delete Test");
            await storage.Delete(fileName);

            // Assert
            Assert.False(File.Exists(fileName));
        }

        [Fact]
        public async void EnumerateEmptyTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var folder = Path.Combine(REF, "_empty");

            // Act
            EnsureTestDirCreated();

            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            var actual = await storage.Enumerate(folder);

            // Assert
            Assert.True(Directory.Exists(folder));
            Assert.Empty(actual);
        }

        [Fact]
        public async void EnumerateTest()
        {
            // Arrange
            var storage = new EmbeddedStorage();
            var data = new string[3] { "Tomato", "Banana", "Plum" };
            var dir = Path.Combine(REF, "_test");

            // Act
            EnsureTestDirCreated();

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

        private void EnsureTestDirCreated()
        {
            if (!Directory.Exists(REF))
                Directory.CreateDirectory(REF);
        }
    }
}
