using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Mocoding.EasyDocDb.Tests")]
namespace Mocoding.EasyDocDb.FileSystem
{
    internal class EmbeddedStorage : IDocumentStorage
    {
        public async Task<string> Read(string @ref)
        {
            try
            {
                if (!File.Exists(@ref))
                    return string.Empty;

                var content = await Task.Run(() => File.ReadAllText(@ref));
                return content;
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to read content from file {@ref}", ex);
            }
        }

        public async Task Write(string @ref, string document)
        {
            try
            {
                await Task.Run(() => File.WriteAllText(@ref, document));
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to write content to file {@ref}", ex);
            }
        }

        public async Task Delete(string @ref)
        {
            try
            {
                if (File.Exists(@ref))
                    await Task.Run(() => File.Delete(@ref));
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to delete file {@ref}", ex);
            }
        }

        public async Task<string[]> Enumerate(string collectionRef)
        {
            try
            {
                if (Directory.Exists(collectionRef))
                    return await Task.Run(() => Directory.EnumerateFiles(collectionRef).ToArray());
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to enumerate files in directory {collectionRef}", ex);
            }

            try
            {
                Directory.CreateDirectory(collectionRef);
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to create directory {collectionRef}", ex);
            }

            return new string[0];
        }

        public string NewRef(string collectionRef, string name)
        {
            return Path.Combine(collectionRef, name);
        }
    }
}
