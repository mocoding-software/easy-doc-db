using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace Mocoding.EasyDocDb.Csv
{
    public class CsvSerializer : IDocumentSerializer
    {
        public string Type => "csv";

        public string Serialize<T>(T data)
            where T : class, new()
        {
            var list = data is IEnumerable enumerable ? enumerable : new[] { data };

            using var stringWriter = new StringWriter();
            using var csv = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);
            csv.WriteRecords(list);
            return stringWriter.ToString();
        }

        public T Deserialize<T>(string content)
            where T : class, new()
        {
            var type = typeof(T);
            var isTypeCollection = type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type);
            var targetType = isTypeCollection ? type.GenericTypeArguments[0] : type;
            using var stringReader = new StringReader(content);
            using var csv = new CsvReader(stringReader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords(targetType);

            return isTypeCollection ? ConstructList<T>(records) : records.FirstOrDefault() as T;
        }

        public T ConstructList<T>(IEnumerable<object> enumerable)
            where T : class, new()
        {
            var t = new T();
            if (!(t is IList array))
                throw new InvalidOperationException($"Type does not implement IList. Only IList derived types are supported.");
            foreach (var item in enumerable)
                array.Add(item);

            return t;
        }
    }
}
