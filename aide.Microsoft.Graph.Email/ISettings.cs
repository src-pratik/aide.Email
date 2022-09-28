namespace aide.Microsoft.Graph.Email
{
    public interface ISettings
    {
        string? ClientId { get; set; }
        string? ClientSecret { get; set; }
        string? TenantId { get; set; }
    }
}