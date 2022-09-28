using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace aide.Microsoft.Graph.Email
{
    public class DaemonService : IDaemonService
    {
        // App-ony auth token credential
        private ClientSecretCredential? _clientSecretCredential;

        private ISettings? _settings;

        // Client configured with user authentication
        private GraphServiceClient? _userClient;

        private ILogger<DaemonService>? _logger;

        public DaemonService(ISettings settings)
        {
            _settings = settings;
            Setup();
        }

        public DaemonService(ISettings settings, ILogger<DaemonService> logger)
        {
            _settings = settings;
            _logger = logger;
            Setup();
        }

        public async Task<bool> HealthCheck(string userid)
        {
            // Ensure client isn't null
            if (_userClient == null)
            {
                _logger?.LogError("Health Check Error : User Client is null.");
                return false;
            }

            try
            {
                var data = await _userClient.Users[userid]
                                   .MailFolders["Inbox"]
                                   .Messages
                                   .Request()
                                   .Top(1)
                                   .GetAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Health Check Error {message}", ex.Message);
                return false;
            }

            return true;
        }

        public async Task SendHTMLMailAsync(string subject, string body, string from, string recipient)
        {
            await SendMailAsync(subject, body, BodyType.Html, from, new string[] { recipient });
        }

        public async Task SendHTMLMailAsync(string subject, string body, string from, string[] recipients)
        {
            await SendMailAsync(subject, body, BodyType.Html, from, recipients);
        }

        public async Task SendTextMailAsync(string subject, string body, string from, string recipient)
        {
            await SendMailAsync(subject, body, BodyType.Text, from, new string[] { recipient });
        }

        public async Task SendTextMailAsync(string subject, string body, string from, string[] recipients)
        {
            await SendMailAsync(subject, body, BodyType.Text, from, recipients);
        }

        private async Task SendMailAsync(string subject, string body, BodyType bodyType, string from, string[] recipients)
        {
            // Ensure client isn't null
            _ = _userClient ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");

            // Create a new message
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    Content = body,
                    ContentType = bodyType
                },
                ToRecipients = recipients.Select(x => new Recipient()
                {
                    EmailAddress = new EmailAddress() { Address = x }
                })
            };

            // Send the message
            await _userClient.Users[from]
                .SendMail(message)
                .Request()
                .PostAsync();
        }

        private void Setup()
        {
            // Ensure settings isn't null
            _ = _settings ??
                throw new System.NullReferenceException("Settings cannot be null");

            if (_clientSecretCredential == null)
            {
                _clientSecretCredential = new ClientSecretCredential(
                    _settings.TenantId, _settings.ClientId, _settings.ClientSecret);
            }

            if (_userClient == null)
            {
                _userClient = new GraphServiceClient(_clientSecretCredential,
                    // Use the default scope, which will request the scopes
                    // configured on the app registration
                    new[] { "https://graph.microsoft.com/.default" });
            }
        }
    }
}