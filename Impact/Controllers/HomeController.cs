using Impact.DAL;
using Impact.Models;
using Impact.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Impact.Controllers
{
	
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
        {
			_context = context;
		}
        public async Task<IActionResult>  Index()
		{
			List<Portfolio> portfolios =await _context.Portfolios.ToListAsync();
			List<Service> services = await _context.Services.ToListAsync();


			HomeVM homeVM = new HomeVM { 
			
			
				Portfolios = portfolios,
				Services = services

			
			};

			return View(homeVM);
		}
	}
}
