using System.Threading.Tasks;

namespace Mocoding.EasyDocDb.CrudSample.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
