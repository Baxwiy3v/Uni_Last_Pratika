using System.ComponentModel.DataAnnotations;

namespace Impact.Areas.Admin.ViewModels;

public class LoginVM
{
	[Required]
	[MaxLength(255)]
	[MinLength(3)]


	public string UserOrEmail { get; set; }
	[Required]

	[DataType(DataType.Password)]

	public string Password { get; set; }

	public bool RememberMe { get; set; }

}
