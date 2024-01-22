using System.ComponentModel.DataAnnotations;

namespace Impact.Areas.Admin.ViewModels;

public class UpdateServiceVM
{

    [Required]
    [MaxLength(75)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]

    public string Description { get; set; }

    [Required]
    public string Icon { get; set; }
}
