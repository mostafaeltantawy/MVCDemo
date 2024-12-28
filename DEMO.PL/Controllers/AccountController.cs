using DEMO.DAL.Models;
using DEMO.PL.Helpers;
using DEMO.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DEMO.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    Fname = model.FName,
                    Lname = model.LName,
                    IsAgree = model.IsAgree,
                };
                var Result = await _userManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }

            }
            return View(model);
        }
        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {
                        var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (Result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Email Or Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not existed");
                }

            }
            return View(model);
        }
        #endregion

        #region SignOut
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("ForgetPassword", model);

            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User != null)
            {
                var ResetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(User);

                var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, token = ResetPasswordToken }, Request.Scheme);


                var email = new Email()
                {
                    Subject = "Reset Password",
                    To = model.Email,
                    Body = ResetPasswordLink
                };
                EmailSettings.SendEmail(email);
                return RedirectToAction(nameof(CheckYourInbox));

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email Is not Exist");
                return View("ForgetPassword", model);

            }



        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);

                var Result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }
    }
}
