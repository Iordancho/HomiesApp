using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Homies.Data;

public class EventParticipant
{
    [Required] 
    public string HelperId { get; set; } = string.Empty;
    [ForeignKey(nameof(HelperId))] 
    public IdentityUser Helper { get; set; } = null!;

    [Required]
    public int EventId { get; set; }
    [ForeignKey(nameof(EventId))] 
    public Event Event { get; set; } = null!;
}