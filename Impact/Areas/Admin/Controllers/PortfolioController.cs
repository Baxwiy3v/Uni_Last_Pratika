using Impact.Areas.Admin.ViewModels;
using Impact.DAL;
using Impact.Models;
using Impact.Utitilies.Extention;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Impact.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Portfolio> portfolios = await _context.Portfolios.ToListAsync();

            return View(portfolios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreatePortfolioVm portfolioVm)
        {
            if (!ModelState.IsValid) return View(portfolioVm);

            bool result = await _context.Portfolios.AnyAsync(p => p.Name.Trim().ToLower() == portfolioVm.Name.Trim().ToLower());

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda name movcuddur");
                return View(portfolioVm);
            }

            if (!portfolioVm.Photo.ValidatorType("image/"))
            {
                ModelState.AddModelError("Photo", "Seklin tipi uygun deyil");
                return View(portfolioVm);
            }
            if (!portfolioVm.Photo.ValidatorSize(4 * 1024))
            {
                ModelState.AddModelError("Photo", "Seklin olcusu uygun deyil");
                return View(portfolioVm);
            }

            string image = await portfolioVm.Photo.CreateFile(_env.WebRootPath, "assets", "img", "Aqil");

            Portfolio portfolio = new Portfolio
            {

                Name = portfolioVm.Name,
                Description = portfolioVm.Description,
                Image = image


            };
            await _context.AddAsync(portfolio);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Portfolio existed = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);

            if (existed == null)
            {
                return NotFound();
            }


            UpdatePortfolioVM portfolioVM = new UpdatePortfolioVM
            {


                Name = existed.Name,
                Description = existed.Description,
                Image = existed.Image

            };

            return View(portfolioVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdatePortfolioVM portfolioVm)
        {
            if (!ModelState.IsValid) return View(portfolioVm);

            if (id <= 0) return BadRequest();

            Portfolio existed = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);

            if (existed == null)
            {
                return NotFound();
            }



            bool result = await _context.Portfolios.AnyAsync(p => p.Name.Trim().ToLower() == portfolioVm.Name.Trim().ToLower() && p.Id!=id);

            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda name movcuddur");
                return View(portfolioVm);
            }

            if (portfolioVm.Photo != null)
            {
                if (!portfolioVm.Photo.ValidatorType("image/"))
                {
                    ModelState.AddModelError("Photo", "Seklin tipi uygun deyil");
                    return View(portfolioVm);
                }
                if (!portfolioVm.Photo.ValidatorSize(4 * 1024))
                {
                    ModelState.AddModelError("Photo", "Seklin olcusu uygun deyil");
                    return View(portfolioVm);
                }

                string newimage = await portfolioVm.Photo.CreateFile(_env.WebRootPath, "assets", "img", "Aqil");

                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "Aqil");

                existed.Image = newimage;
            }

            existed.Description = portfolioVm.Description;
            existed.Name = portfolioVm.Name;


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {


            if (id <= 0) return BadRequest();

            Portfolio existed = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);

            if (existed == null)
            {
                return NotFound();
            }

            _context.Portfolios.Remove(existed);

            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "Aqil");


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");   

        }
    }
}
