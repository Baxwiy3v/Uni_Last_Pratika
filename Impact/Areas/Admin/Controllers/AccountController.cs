using Impact.Areas.Admin.ViewModels;
using Impact.Models;
using Impact.Utitilies.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Impact.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _manager;
		private readonly SignInManager<AppUser> _signIn;
		private readonly RoleManager<IdentityRole> _role;

		public AccountController(UserManager<AppUser> manager, SignInManager<AppUser> signIn, RoleManager<IdentityRole> role)
		{
			_manager = manager;
			_signIn = signIn;
			_role = role;
		}
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM registerVM)
		{
			if (!ModelState.IsValid) return View(registerVM);

			registerVM.Name = registerVM.Name.Trim();
			registerVM.Surname = registerVM.Surname.Trim();


			string name = Char.ToUpper(registerVM.Name[0]) + registerVM.Name.Substring(1).ToLower();
			string surname = Char.ToUpper(registerVM.Surname[0]) + registerVM.Surname.Substring(1).ToLower();


			AppUser user = new AppUser
			{

				Name = registerVM.Name,
				Surname = registerVM.Surname.Trim(),
				UserName = registerVM.Name,
				Email = registerVM.Email

			};

			var manager = await _manager.CreateAsync(user, registerVM.Password);

			if (!manager.Succeeded)
			{
				foreach (var erro in manager.Errors)
				{
					ModelState.AddModelError(String.Empty, erro.Description);
				}
				return View(registerVM);
			}

			await _signIn.SignInAsync(user, isPersistent: false);

			return RedirectToAction("Index", "Home", new { Area = " " });

		}




		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Login(LoginVM loginVM)
		{
			if (!ModelState.IsValid) return View(loginVM);

			var user = await _manager.FindByNameAsync(loginVM.UserOrEmail);

			if (user == null)
			{
				user = await _manager.FindByEmailAsync(loginVM.UserOrEmail);

				if (user == null)
				{
					ModelState.AddModelError(String.Empty, "User,Emial ve ya Password sehvdir");
					return View(loginVM);
				}

			}


			var result = await _signIn.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

			if (result.IsLockedOut)
			{
				ModelState.AddModelError(String.Empty, "Heddinden artiq cehd biraz sonra yeniden yoxlayin");
				return View(loginVM);

			}

			if (!result.Succeeded)
			{
				ModelState.AddModelError(String.Empty, "User,Emial ve ya Password sehvdir");
				return View(loginVM);
			}



			return RedirectToAction("Index", "Home", new { Area = " " });
		}

		public async Task<IActionResult> Logout()
		{
			await _signIn.SignOutAsync();

			return RedirectToAction("Index", "Home", new { Area = " " });
		}


        public async Task<IActionResult> CreateRoles()
        {

			foreach (var role in Enum.GetValues(typeof(UserRole)))
			{
				await _role.CreateAsync(new IdentityRole
				{
					Name=role.ToString(),

				});

			}
            return RedirectToAction("Index", "Home", new { Area = " " });

        }
    }
}
