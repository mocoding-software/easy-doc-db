using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.Core
{
    internal class DocumentsCollection<T> : IDocumentCollection<T> where T : class, new()
    {
        private ImmutableList<IDocument<T>> _list;
        private readonly IDocumentStorage _storage;
        private readonly IDocumentSerializer _serializer;
        private readonly string _collectionRef;
        private readonly object _accessToList = new object();

        public DocumentsCollection(string collectionRef, IDocumentStorage storage, IDocumentSerializer serializer)
        {
            _list = ImmutableList.Create<IDocument<T>>();
            _storage = storage;
            _serializer = serializer;
            _collectionRef = collectionRef;
        }

        internal async Task Init()
        {
            var builder = _list.ToBuilder();
            var files = await _storage.Enumerate(_collectionRef);

            foreach (var file in files)
            {
                var doc = new Document<T>(file, _storage, _serializer, OnElementDeleted);
                await doc.Init();
                builder.Add(doc);
            }

            _list = builder.ToImmutableList();
        }

        public IDocument<T> New()
        {
            var guid = Guid.NewGuid();
            var docName = $"{guid}.{_serializer.Type}";
            var docRef = _storage.NewRef(_collectionRef, docName);
            return new Document<T>(docRef, _storage, _serializer, OnElementDeleted, OnElementSaved);
        }

        private void OnElementSaved(IDocument<T> arg)
        {
            _list = _list.Add(arg);
        }

        private void OnElementDeleted(IDocument<T> arg)
        {
            _list = _list.Remove(arg);
        }

        public int Count => _list.Count;

        public IEnumerator<IDocument<T>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
