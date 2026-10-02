namespace SimpleChat.Models;

public class AppSettings
{
    public string AppName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int MaxItemsPerPage { get; set; }
}