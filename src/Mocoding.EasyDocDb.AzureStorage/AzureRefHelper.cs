using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mocoding.EasyDocDb.AzureStorage
{
    public class AzureRefHelper
    {
        public AzureRefHelper(string connection)
        {
            var listConnections = connection.Split('/', '\\').ToList();
            Init(listConnections);
        }

        public string Container { get; private set; }
        public string FileName { get; private set; }

        private void Init(IReadOnlyList<string> connection)
        {
            if (connection.Count == 2)
            {
                Container = connection[0];
                FileName = connection[1];
            }

            if (connection.Count == 1)
            {
                Container = connection[0];
            }
        }
    }
}
