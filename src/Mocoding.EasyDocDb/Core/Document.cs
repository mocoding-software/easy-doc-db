using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.Core
{
    internal class Document<T> : IDocument<T> where T : class, new()
    {
        private const int TIMEOUT = 1000 * 2; // 2 seconds timeout to save a document.

        internal readonly string _ref;

        private readonly IDocumentSerializer _serializer;
        private readonly IDocumentStorage _storage;

        private Action<IDocument<T>> _onSave;
        private Action<IDocument<T>> _onDelete;
        
        private readonly object _dataAccess = new object();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public Document(string @ref, IDocumentStorage storage, IDocumentSerializer serializer, Action<IDocument<T>> onDelete = null, Action<IDocument<T>> onSave = null)
        {
            _storage = storage;
            _ref = @ref;
            _serializer = serializer;
            _onDelete = onDelete;
            _onSave = onSave;
            Data = new T();
        }

        internal async Task Init()
        {
            var content = await _storage.Read(_ref);
            Data = _serializer.Deserialize<T>(content) ?? new T();
        }

        public T Data { get; private set; }

        public async Task SyncUpdate(Action<T> updateAction)
        {
            if (!await _semaphore.WaitAsync(TIMEOUT))
                throw new EasyDocDbException($"Timeout! Can't get exclusive access to document.");

            try
            {
                updateAction(Data);
                await SaveInternal();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task Save()
        {
            if(!await _semaphore.WaitAsync(TIMEOUT))
                throw new EasyDocDbException($"Timeout! Can't get exclusive access to document.");

            try
            {
                await SaveInternal();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task SaveInternal()
        {
            var content = _serializer.Serialize(Data);
            await _storage.Write(_ref, content);
            _onSave?.Invoke(this);
            _onSave = null; // intended for single use.
        }

        public async Task Delete()
        {
            if (!await _semaphore.WaitAsync(TIMEOUT))
                throw new EasyDocDbException($"Timeout! Can't get exclusive access to document.");

            try
            {
                await _storage.Delete(_ref);
                _onDelete?.Invoke(this);
                _onDelete = null; // intended for single use.
                Data = new T(); // reset the data.
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
