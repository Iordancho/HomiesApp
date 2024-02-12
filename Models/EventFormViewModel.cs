using System.ComponentModel.DataAnnotations;
using Homies.Data;
using static Homies.Data.DataConstants;

namespace Homies.Models;

public class EventFormViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = RequireErrorMessage)]
    [StringLength(EventNameMax, MinimumLength = EventNameMin, ErrorMessage = StringLenghtErrorMessage)]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = RequireErrorMessage)]
    [StringLength(DescriptionMax, MinimumLength = DescriptionMin, ErrorMessage = StringLenghtErrorMessage)]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = RequireErrorMessage)]
    public string Start { get; set; } = string.Empty;
    
    [Required(ErrorMessage = RequireErrorMessage)]
    public string End { get; set; } = string.Empty;
    
    [Required(ErrorMessage = RequireErrorMessage)]
    public int TypeId { get; set; }
    public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
}