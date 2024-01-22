using System.ComponentModel.DataAnnotations;

namespace Impact.Areas.Admin.ViewModels;

public class CreatePortfolioVm
{
    [Required]
    [MinLength(3)]
    [MaxLength(75)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public IFormFile Photo { get; set; }
}
