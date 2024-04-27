using Route.C41.G02.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Route.C41.G02.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required !!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is Required !!")]
        public string Code { get; set; } 
        [Display(Name = "Date Of Creation")] 

        public DateTime DateOfCreation { get; set; }


        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
