using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb
{
    /// <summary>
    /// Allows
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Initializes the document that is stored in a single file.
        /// It will read the file if it is already exists.
        /// </summary>
        /// <typeparam name="T">Type of the document.</typeparam>
        /// <param name="conn">Path to file or other reference to where the file is stored on.</param>
        /// <returns></returns>
        Task<IDocument<T>> Init<T>(string conn)
            where T : class, new();

        /// <summary>
        /// Initializes the collection of documents that is stored as a set of files.
        /// It will read all existing files and populate documents from them.
        /// </summary>
        /// <typeparam name="T">Type of the document.</typeparam>
        /// <param name="conn">Path to folder where documents are stored</param>
        /// <returns></returns>
        Task<IDocumentCollection<T>> InitCollection<T>(string conn)
            where T : class, new();
    }

    /// <summary>
    /// Represents a collection of documents and allows certain operations to be performed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDocumentCollection<T>
        where T : class, new()
    {
        /// <summary>
        /// Returns all documents.
        /// </summary>
        /// <value>
        /// The current snapshot of the collection.
        /// </value>
        ImmutableArray<IDocument<T>> Documents { get; }

        /// <summary>
        /// Creates a new document but does not add it to the collection.
        /// The document is added when the document is saved.
        /// </summary>
        /// <returns></returns>
        IDocument<T> New();
    }

    /// <summary>
    /// Represents a document and allows certain operations to be performed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDocument<out T>
        where T : new()
    {
        /// <summary>
        /// Returns the document content.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        T Data { get; }

        /// <summary>
        /// Allows to update document data in a thread-safe manner.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SyncUpdate(Action<T> data);

        /// <summary>
        /// Allows saving the document data in a thread-safe manner.
        /// </summary>
        /// <returns></returns>
        Task Save();

        /// <summary>
        /// Allows deleting the document data in a thread-safe manner.
        /// </summary>
        /// <returns></returns>
        Task Delete();
    }

    /// <summary>
    /// A facade for serializer used to serialize and deserialize content of the document.
    /// </summary>
    public interface IDocumentSerializer
    {
        /// <summary>
        /// Identifies the serializer by returning an extension to be used to save documents as files.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        string Type { get; }

        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        string Serialize<T>(T data)
            where T : class, new();

        /// <summary>
        /// Deserializes the specified content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        T Deserialize<T>(string content)
            where T : class, new();
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
