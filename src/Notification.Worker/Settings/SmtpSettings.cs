using System.ComponentModel.DataAnnotations;

namespace Notification.Worker.Settings;

public class SmtpSettings
{
    public const string SectionName = "Smtp";

    [Required]
    public string Host { get; set; }
    
    [Required]
    public int Port { get; set; }
    
    public bool EnableSsl { get; set; }

    [Required]
    [EmailAddress]
    public string FromAddress { get; set; }
    
    [Required]
    public string FromName { get; set; }
}