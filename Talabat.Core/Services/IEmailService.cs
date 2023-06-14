using Talabat.Core.Entities.EmailSettings;

namespace Talabat.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
    }
}
