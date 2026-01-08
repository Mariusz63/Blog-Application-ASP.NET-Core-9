using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }


    //Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        //Check for validation error
        if (!ModelState.IsValid)
        {
            //Create Identity user object
            var user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email
            };

            //Create user in the database
            var result = await _userManager.CreateAsync(user, model.Password);

            //If user creation is successfully done
            if (result.Succeeded)
            {
                //if the User role exist in data base
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, true);

                return RedirectToAction("Index", "Post");
            }


            return RedirectToAction("Index", "Post");
        }
        return View(model);
    }

    //Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        //Check for validation error
        if (ModelState.IsValid)
        {
            //Sign in the user
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View(model);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            //If sign in is successfully done
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Post");
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        }
        return View(model);
    }

    //Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Post");
    }
}