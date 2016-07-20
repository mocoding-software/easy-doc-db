using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.Core
{
    internal class DocumentsCollection<T> : IDocumentCollection<T> where T : class, new()
    {
        private readonly ImmutableArray<IDocument<T>>.Builder _builder;
        private readonly IDocumentStorage _storage;
        private readonly IDocumentSerializer _serializer;
        private readonly string _collectionRef;
        private readonly object _syncRoot = new object();

        public DocumentsCollection(string collectionRef, IDocumentStorage storage, IDocumentSerializer serializer)
        {
            _builder = ImmutableArray.CreateBuilder<IDocument<T>>();
            Documents = _builder.ToImmutable();
            _storage = storage;
            _serializer = serializer;
            _collectionRef = collectionRef;
        }

        internal async Task Init()
        {
            var files = await _storage.Enumerate(_collectionRef);
            _builder.Capacity = files.Length;

            foreach (var file in files)
            {
                var doc = new Document<T>(file, _storage, _serializer, OnElementDeleted);
                await doc.Init();
                _builder.Add(doc);
            }

            Documents = _builder.ToImmutable();
        }
        
        public ImmutableArray<IDocument<T>> Documents { get; private set; }

        public IDocument<T> New()
        {
            var guid = Guid.NewGuid();
            var docName = $"{guid}.{_serializer.Type}";
            var docRef = _storage.NewRef(_collectionRef, docName);
            return new Document<T>(docRef, _storage, _serializer, OnElementDeleted, OnElementSaved);
        }

        private void OnElementSaved(IDocument<T> arg)
        {
            //lock is still required to avoid possible data loss when OnElementSaved is invoked from different threads.
            lock (_syncRoot)
            {
                _builder.Add(arg);
                Documents = _builder.ToImmutable();
            }
        }

        private void OnElementDeleted(IDocument<T> arg)
        {
            //lock is still required to avoid possible data loss when OnElementDeleted is invoked from different threads.
            lock (_syncRoot)
            {
                _builder.Remove(arg);
                Documents = _builder.ToImmutable();
            }
        }
       
    }
}
