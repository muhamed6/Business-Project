using System.ComponentModel.DataAnnotations;

namespace Route.C41.G02.PL.ViewModels.Account
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "New Password is required")]
        [MinLength(5,ErrorMessage ="Minimum Password Length is 5 ")]
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password doesnot match with New password")]

        public string ConfirmPassword { get; set; }

    }
}
