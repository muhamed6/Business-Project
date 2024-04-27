﻿using System.ComponentModel.DataAnnotations;

namespace Route.C41.G02.PL.ViewModels.Account
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }


		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		public bool RememberMe { get; set; }
	}
}
