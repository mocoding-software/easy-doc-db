using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.Core
{
    internal class Document<T> : IDocument<T>
        where T : class, new()
    {
        private const int Timeout = 1000 * 2; // 2 seconds timeout to save a document.
        private readonly string _ref;
        private readonly IDocumentSerializer _serializer;
        private readonly IDocumentStorage _storage;
        private readonly object _dataAccess = new object();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private Action<IDocument<T>> _onSave;
        private Action<IDocument<T>> _onDelete;

        public Document(string @ref, IDocumentStorage storage, IDocumentSerializer serializer, Action<IDocument<T>> onDelete = null, Action<IDocument<T>> onSave = null, T initialData = null)
        {
            _storage = storage;
            _ref = @ref;
            _serializer = serializer;
            _onDelete = onDelete;
            _onSave = onSave;
            Data = initialData ?? new T();
        }

        public T Data { get; private set; }

        public Task SyncUpdate(Action<T> updateAction)
        {
            return Synchronize(async () =>
            {
                updateAction(Data);
                await SaveInternal();
            });
        }

        public Task SyncUpdate(T newData)
        {
            return Synchronize(async () =>
            {
                Data = newData;
                await SaveInternal();
            });
        }

        public Task Save()
        {
            return Synchronize(SaveInternal);
        }

        public Task Delete()
        {
            return Synchronize(async () =>
            {
                await _storage.Delete(_ref);
                _onDelete?.Invoke(this);
                _onDelete = null; // intended for single use.
                Data = new T(); // reset the data.
            });
        }

        internal async Task Init()
        {
            var content = await _storage.Read(_ref);
            Data = _serializer.Deserialize<T>(content) ?? new T();
        }

        private async Task SaveInternal()
        {
            var content = _serializer.Serialize(Data);
            await _storage.Write(_ref, content);
            _onSave?.Invoke(this);
            _onSave = null; // intended for single use.
        }

        private async Task Synchronize(Func<Task> criticalSectionFunc)
        {
            if (!await _semaphore.WaitAsync(Timeout))
                throw new EasyDocDbException($"Timeout! Can't get exclusive access to document.");

            try
            {
                await criticalSectionFunc();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
