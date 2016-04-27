using System;

namespace Mocoding.EasyDocDb
{
    public class StorageException : Exception
    {
        public StorageException(string message) : base(message) { }

        public StorageException(string message, Exception inner) : base(message, inner) { }
    }

    public class EasyDocDbException : Exception
    {
        public EasyDocDbException(string message) : base(message) { }

        public EasyDocDbException(string message, Exception inner) : base(message, inner) { }
    }
}
