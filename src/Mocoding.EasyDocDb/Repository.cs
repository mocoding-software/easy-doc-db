using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mocoding.EasyDocDb.Core;

[assembly: InternalsVisibleTo("Mocoding.EasyDocDb.Tests")]

namespace Mocoding.EasyDocDb
{
    public class Repository : IRepository
    {
        private readonly IDocumentSerializer _serializer;
        private readonly IDocumentStorage _storage;

        public Repository(IDocumentSerializer serializer, IDocumentStorage storage)
        {
            _serializer = serializer;
            _storage = storage;
        }

        public async Task<IDocument<T>> Init<T>(string conn)
            where T : class, new()
        {
            var doc = new Document<T>(conn, _storage, _serializer);
            await doc.Init();
            return doc;
        }

        public async Task<IDocumentCollection<T>> InitCollection<T>(string conn)
            where T : class, new()
        {
            var collection = new DocumentsCollection<T>(conn, _storage, _serializer);
            await collection.Init();
            return collection;
        }
    }
}
