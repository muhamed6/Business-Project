using System.ComponentModel.DataAnnotations;

namespace Route.C41.G02.PL.ViewModels.Account
{
	public class SignUpViewModel
	{


		[Required(ErrorMessage = "UserName is required")]

		public string Username { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage ="First Name is Required!")]
		[Display(Name ="First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is Required!")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage ="Password is required")]
		[DataType(DataType.Password)]

		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage ="Confirm Password doesnot match with password")]

		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; } 
	}
}
