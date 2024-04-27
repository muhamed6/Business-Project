using Route.C41.G02.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Route.C41.G02.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars!!")]
        [MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars!!")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int Age { get; set; }


        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
        , ErrorMessage = "address must be like 123-street-city-country")] 

        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Phone]

        [Display(Name = "phone Number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }

        public Gender Gender { get; set; }

        public EmpType EmployeeType { get; set; }

        [Display(Name = "Hiring Date")]
      
     


        public Department Department { get; set; }
        public int? DepartmentId { get; set; }
        [Required(ErrorMessage ="Image Is Required")]
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
    }
}
