using Microsoft.Graph;
using System.Threading.Tasks;

namespace aide.Microsoft.Graph.Email
{
    public interface IDaemonService
    {
        Task<bool> HealthCheck(string userid);

        Task SendHTMLMailAsync(string subject, string body, string from, string recipient, FileAttachment[]? attachments = null);

        Task SendHTMLMailAsync(string subject, string body, string from, string[] recipients, FileAttachment[]? attachments = null);

        Task SendTextMailAsync(string subject, string body, string from, string recipient, FileAttachment[]? attachments = null);

        Task SendTextMailAsync(string subject, string body, string from, string[] recipients, FileAttachment[]? attachments = null);
    }
}