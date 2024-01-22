using System.ComponentModel.DataAnnotations;

namespace Impact.Areas.Admin.ViewModels;

public class UpdatePortfolioVM

{

    [Required]
    [MinLength(3)]
    [MaxLength(75)]
    public string Name { get; set; }
    [Required]
    [MaxLength(255)]
    public string Description { get; set; }

    public string? Image { get; set; }

    public IFormFile? Photo { get; set; }
}
