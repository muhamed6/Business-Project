using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.Services.EmailSender;
using Route.C41.G02.PL.ViewModels.Account;
using System.Threading.Tasks;

namespace Route.C41.G02.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;

		public AccountController( UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender,
			IConfiguration configuration
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
			_configuration = configuration;
		}
		#region Sign Up

		public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
         public async Task <IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid) 
            {

				var user= await _userManager.FindByNameAsync(model.Username);

				 if(user is null) 
                {
					user = new ApplicationUser()
					{
						FName = model.FirstName,
						LName = model.LastName,
						UserName = model.Username,
						Email = model.Email,
						IsAgree = model.IsAgree,
					};

					var result=await _userManager.CreateAsync(user, model.Password);
				
				   if(result.Succeeded) 
					return RedirectToAction(nameof(SignIn));

				   foreach(var error in result.Errors)
						ModelState.AddModelError(string.Empty,error.Description);
				}

				ModelState.AddModelError(string.Empty, "this user is already in use for another account");

			}
			return View(model);

        }





		#endregion

		#region SignIn

		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);

					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);// PasswordSignInAsync to generate token and store token in cookie storage 


						if (result.IsLockedOut)
							ModelState.AddModelError(string.Empty, "Your Account is Locked!!");

						if (result.Succeeded)
							return RedirectToAction(nameof(HomeController.Index), "Home");

						if (result.IsNotAllowed)
							ModelState.AddModelError(string.Empty, "Your Account is not Confirmed yet!!");



					}
				}
				ModelState.AddModelError(string.Empty, "Invalid Login");
			}


			return View(model);
		}

		#endregion

		#region Sign Out

		public  async new Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync(); // delete  token from cookie storage
			return RedirectToAction(nameof(SignIn));
		}
        #endregion


        #region ForgetPassword

        public IActionResult ForgetPassword()
        {
            return View();
        }


		
		public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user); // unique token for this user

					// Generate the path part of the URL without protocol and host
					var resetPasswordPath = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken });

					// Manually construct the full URL with protocol, host, and port
					var resetPasswordUrl = $"{Request.Scheme}://localhost:44330{resetPasswordPath}";

					await _emailSender.SendAsync(
						from: _configuration["EmailSettings:SenderEmail"],
						recipients: model.Email,
						subject: "Reset Your Password",
						body: resetPasswordUrl);

					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "There is No Account With this Email!!");
			}

			return View(model);
		}


		public IActionResult CheckYourInbox()
        {
            return View();
        }

		#endregion


		#region ResetPassword

		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["Token"] = token;
			TempData["Email"] = email;

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
            if(ModelState.IsValid) 
			{
			 var email= TempData["Email"] as string;
			 var token = TempData["Token"] as string;

				var user=await _userManager.FindByEmailAsync(email);

				if(user is not null) 
				{
				 await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
					return RedirectToAction(nameof(SignIn));
				}
				ModelState.AddModelError(string.Empty, "Url Is Not Valid");
			}
		
		return View(model);
		}
		#endregion


	}
}
