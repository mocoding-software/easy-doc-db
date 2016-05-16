using System.IO;

namespace Mocoding.EasyDocDb.Tests.Helpers
{
    public class TestDir
    {
        public static void EnsureCreated(string @ref)
        {
            if (!Directory.Exists(@ref))
                Directory.CreateDirectory(@ref);
        }
    }
}
