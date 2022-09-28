namespace aide.Microsoft.Graph.Email
{
    public class Settings : ISettings
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? TenantId { get; set; }
    }
}