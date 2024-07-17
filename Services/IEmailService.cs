using API_Movies.Helpers;

namespace API_Movies.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
