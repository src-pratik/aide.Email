// See https://aka.ms/new-console-template for more information
using aide.Microsoft.Graph.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System.Text;

Settings settings;
string userId;

LoadSettings();

var daemonService = new DaemonService(settings);

int choice = -1;

while (choice != 0)
{
    Console.WriteLine("Please choose one of the following options:");
    Console.WriteLine("0. Exit");
    Console.WriteLine("1. Health Check");
    Console.WriteLine("2. Send Text Mail");
    try
    {
        choice = int.Parse(Console.ReadLine() ?? string.Empty);
    }
    catch (System.FormatException)
    {
        // Set to invalid value
        choice = -1;
    }

    switch (choice)
    {
        case 0:
            // Exit the program
            Console.WriteLine("Goodbye...");
            break;

        case 1:
            var output = await daemonService.HealthCheck(userId);
            Console.WriteLine($"Health Check Status {output}");
            break;

        case 2:
            try
            {
                FileAttachment attachment = new FileAttachment();
                attachment.ContentType = "text/plain";
                attachment.Name = "Sample.txt";
                attachment.ContentBytes = Encoding.ASCII.GetBytes("Testing data from the file.");
                await daemonService.SendTextMailAsync("Testing : With Attachment. External Mail address", "This is a test mail. Please let me know you get the attachment in the mail. Text file with content.", userId, userId, new FileAttachment[] { attachment });
                Console.WriteLine($"Mail Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send Mail Error {ex.Message}");
            }
            break;

        default:
            Console.WriteLine("Invalid choice! Please try again.");
            break;
    }
}

void LoadSettings()
{
    // Load settings
    IConfiguration config = new ConfigurationBuilder()
        // appsettings.json is required
        .AddJsonFile("appsettings.json", optional: false)
        // appsettings.Development.json" is optional, values override appsettings.json
        // .AddJsonFile($"appsettings.Development.json", optional: true)
        // User secrets are optional, values override both JSON files
        // .AddUserSecrets<Program>()
        .Build();

    settings = config.GetRequiredSection("GraphMailDaemon").Get<Settings>();
    userId = config.GetValue<string>("userid");
}