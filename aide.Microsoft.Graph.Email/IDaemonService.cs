using System.Threading.Tasks;

namespace aide.Microsoft.Graph.Email
{
    public interface IDaemonService
    {
        Task<bool> HealthCheck(string userid);

        Task SendHTMLMailAsync(string subject, string body, string from, string recipient);

        Task SendHTMLMailAsync(string subject, string body, string from, string[] recipients);

        Task SendTextMailAsync(string subject, string body, string from, string recipient);

        Task SendTextMailAsync(string subject, string body, string from, string[] recipients);
    }
}