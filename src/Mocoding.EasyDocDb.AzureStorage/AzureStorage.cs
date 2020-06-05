using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
namespace Mocoding.EasyDocDb.AzureStorage
{
    public class AzureStorage : IDocumentStorage
    {
        private readonly CloudBlobClient _blobClient;

        public AzureStorage(AzureStorageSettings storageSettings)
        {
            var storageAccount =
                new CloudStorageAccount(
                    new StorageCredentials(storageSettings.AzureAccount, storageSettings.AzureKey), false);

            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> Read(string @ref)
        {
            var refHelper = new AzureRefHelper(@ref);
            try
            {
                var blobContainer = _blobClient.GetContainerReference(refHelper.Container);

                if (!await blobContainer.ExistsAsync())
                    return string.Empty;

                var blob = blobContainer.GetBlockBlobReference(refHelper.FileName);
                if (!await blob.ExistsAsync())
                    return string.Empty;

                using (var blobRead = await blob.OpenReadAsync())
                {
                    var bytes = new BinaryReader(blobRead).ReadBytes((int)blobRead.Length);
                    return Encoding.ASCII.GetString(bytes);
                }
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to read content from blob {@ref}", ex);
            }
        }

        public async Task Write(string @ref, string document)
        {
            var refHelper = new AzureRefHelper(@ref);
            try
            {
                var blobContainer = _blobClient.GetContainerReference(refHelper.Container);

                if (!await blobContainer.ExistsAsync())
                    await blobContainer.CreateIfNotExistsAsync();

                var blob = blobContainer.GetBlockBlobReference(refHelper.FileName);
                await blob.UploadTextAsync(document);
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to write content from blob {@ref}", ex);
            }
        }

        public async Task Delete(string @ref)
        {
            var refHelper = new AzureRefHelper(@ref);
            var blobContainer = _blobClient.GetContainerReference(refHelper.Container);

            if (!await blobContainer.ExistsAsync())
                return;

            var blob = blobContainer.GetBlockBlobReference(refHelper.FileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string[]> Enumerate(string collectionRef)
        {
            var refHelper = new AzureRefHelper(collectionRef);

            var blobContainer = _blobClient.GetContainerReference(refHelper.Container);

            if (!await blobContainer.ExistsAsync())
                throw new ArgumentException("Container doesn't exist");

            var blobResultSegment = await blobContainer.ListBlobsSegmentedAsync(null, null);
            var files = from CloudBlob blobResult in blobResultSegment.Results
                        select blobResult.Name;
            return files.ToArray();
        }

        public string NewRef(string collectionRef, string name)
        {
            return collectionRef + "/" + name;
        }
    }
}
