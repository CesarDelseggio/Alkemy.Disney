using System.Threading.Tasks;

namespace Alkemy.Disney.Services.Email
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email, string subject, string htmlContent, string plainTextContent = "");
    }
}
