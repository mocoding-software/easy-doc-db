using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb
{
    public interface IRepository
    {
        Task<IDocument<T>> Load<T>(string conn) where T : class, new();

        Task<IDocumentCollection<T>> LoadCollection<T>(string conn) where T : class, new();
    }

    public interface IDocumentCollection<out T> where T : class, new()
    {
        IEnumerable<IDocument<T>> All();
        IDocument<T> New();
    }

    public interface IDocument<out T> where T : new()
    {
        T Data { get; }

        Task SyncUpdate(Action<T> data);

        Task Save();

        Task Delete();
    }

    public interface IDocumentSerializer
    {
        string Type { get; }

        string Serialize<T>(T data) where T : class, new();

        T Deserialize<T>(string content) where T : class, new();
    }

    /// <summary>
    /// Abstraction layer over actual storage where documents are stored.
    /// </summary>
    public interface IDocumentStorage
    {
        /// <summary>
        /// Called when it is required to read content of document using the specified reference.
        /// </summary>
        /// <param name="ref">The reference.</param>
        /// <returns>Task to return a document.</returns>
        Task<string> Read(string @ref);

        /// <summary>
        /// Called when it is required to write content of document using using the specified reference.
        /// </summary>
        /// <param name="ref">The reference.</param>
        /// <param name="document">The document.</param>
        /// <returns>Task of writing operation.</returns>
        Task Write(string @ref, string document);

        /// <summary>
        /// Called when it is required to delete content of document using the specified reference.
        /// </summary>
        /// <param name="ref">The reference.</param>
        /// <returns>Task of deleting operation.</returns>
        Task Delete(string @ref);

        /// <summary>
        /// Called when it is required to enumerate documents using specified collection reference.
        /// </summary>
        /// <param name="collectionRef">The collection reference.</param>
        /// <returns></returns>
        Task<string[]> Enumerate(string collectionRef);

        /// <summary>
        /// Creates new reference to the document in collection.
        /// </summary>
        /// <param name="collectionRef">The collection reference.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string NewRef(string collectionRef, string name);
    }
}
