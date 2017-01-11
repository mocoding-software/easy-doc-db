namespace Mocoding.EasyDocDb.FileSystem
{
    public class EmbeddedRepository : Repository
    {
        public EmbeddedRepository(IDocumentSerializer serializer)
            : base(serializer, new EmbeddedStorage())
        {
        }

        public EmbeddedRepository(IDocumentSerializer serializer, IDocumentStorage storage)
            : base(serializer, storage)
        {
        }
    }
}
