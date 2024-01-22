using Impact.Models;
using Microsoft.EntityFrameworkCore;

namespace Impact.ViewModels
{
	public class HomeVM
	{
		public List<Portfolio> Portfolios { get; set; }

		public List<Service> Services { get; set; }
	}
}
