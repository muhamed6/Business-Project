using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.BLL.Repositories;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.Helpers;
using Route.C41.G02.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Route.C41.G02.PL.Controllers
{
	[Authorize]

	public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeController(
         IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchInp)
        {
            var employees =Enumerable.Empty<Employee>();

            var employeeRepo=_unitOfWork.Repository<Employee>()as EmployeeRepository;

            if (string.IsNullOrEmpty(searchInp))
             employees = await _unitOfWork.Repository<Employee>().GetAllAsync();

            else         
              employees = employeeRepo.SearchEmployeeByName(searchInp.ToLower());
            
            var mappedEmps=_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmps);




        }

        [HttpGet]
        public IActionResult Create()
        {
         
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Create(EmployeeViewModel employeeVM)
        {
            
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = await DocumentSetting.UploadFile(employeeVM.Image, "images");


                var mappedEmp =_mapper.Map<EmployeeViewModel,Employee>(employeeVM); 

                _unitOfWork.Repository<Employee>().Add(mappedEmp);
                var count = await _unitOfWork.Complete();

                if (count > 0)

                {
                    TempData["Message"] = "Employee is Created Successfully";
                  
                }

                else

                    TempData["Message"] = "An Error  Has Occured, Employee Not Created ";
              
                return RedirectToAction(nameof(Index));
            }

            return View(employeeVM); 
        }

        [HttpGet]

        public async Task<IActionResult> Details(int? id, string viewName = "Details") 
        {


            if (!id.HasValue)
                return BadRequest(); 

            var employee =await _unitOfWork.Repository<Employee>().GetAsync(id.Value);

            var mappedEmp =_mapper.Map<Employee, EmployeeViewModel> (employee); 

            if (employee is null)
                return NotFound(); 
            
            if(viewName.Equals("Delete", StringComparison.OrdinalIgnoreCase))
            TempData["ImageName"]=employee.ImageName;
             

            return View(viewName, mappedEmp);


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

        
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)

        {

            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employeeVM);

            try
            {
                employeeVM.ImageName =await DocumentSetting.UploadFile(employeeVM.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);     
                _unitOfWork.Repository<Employee>().Update(mappedEmp);
               await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating The Department");

                return View(employeeVM);
            }

        }

        [HttpGet]
        public async  Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {

            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmp=_mapper.Map<EmployeeViewModel, Employee>(employeeVM); 
                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                int count= await _unitOfWork.Complete();
                if(count > 0)
                {
                    DocumentSetting.DeleteFile(employeeVM.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVM);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating The Department");

                return View(employeeVM);

            }
        }

    }
}
