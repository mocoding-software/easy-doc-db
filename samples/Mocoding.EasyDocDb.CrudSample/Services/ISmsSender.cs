using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.CrudSample.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
