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
    /// <summary>
    /// Generic repository implementation that uses <see cref="IDocumentSerializer"/>
    /// and <see cref="IDocumentStorage"/> to manipulate documents.
    /// </summary>
    /// <seealso cref="Mocoding.EasyDocDb.IRepository" />
    public class Repository : IRepository
    {
        private readonly IDocumentSerializer _serializer;
        private readonly IDocumentStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="storage">The storage.</param>
        public Repository(IDocumentSerializer serializer, IDocumentStorage storage)
        {
            _serializer = serializer;
            _storage = storage;
        }

        /// <summary>
        /// Initializes the document that is stored in a single file.
        /// It will read the file if it is already exists.
        /// </summary>
        /// <typeparam name="T">Type of the document.</typeparam>
        /// <param name="conn">Path to file or other reference to where the file is stored on.</param>
        /// <returns>
        /// Document reference.
        /// </returns>
        public async Task<IDocument<T>> Init<T>(string conn)
            where T : class, new()
        {
            var doc = new Document<T>(conn, _storage, _serializer);
            await doc.Init();
            return doc;
        }

        /// <summary>
        /// Initializes the collection of documents that is stored as a set of files.
        /// It will read all existing files and populate documents from them.
        /// </summary>
        /// <typeparam name="T">Type of the document.</typeparam>
        /// <param name="conn">Path to folder where documents are stored.</param>
        /// <returns>
        /// Reference to a collection of documents.
        /// </returns>
        public async Task<IDocumentCollection<T>> InitCollection<T>(string conn)
            where T : class, new()
        {
            var collection = new DocumentsCollection<T>(conn, _storage, _serializer);
            await collection.Init();
            return collection;
        }
    }
}
