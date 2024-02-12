using System.ComponentModel.DataAnnotations;

namespace Homies.Data;

public class Type
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(DataConstants.TypeNameMax)]
    public string Name { get; set; }
    
    public IEnumerable<Event> Events { get; set; } = new List<Event>();
}