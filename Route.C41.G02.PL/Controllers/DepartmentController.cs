using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Route.C41.G02.PL.Controllers
{
	[Authorize]

	public class DepartmentController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public DepartmentController(
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
               IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }

        public async Task <IActionResult> Index()
        {
            var departments=await _unitOfWork.Repository<Department>().GetAllAsync();
            var mappedDeps = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);


            return View(mappedDeps); 



        }

        [HttpGet]
        public IActionResult Create() 
        {

          return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM) 
        {
            if(ModelState.IsValid) 
            { 
                var mappedDep=_mapper.Map<DepartmentViewModel,Department>(departmentVM);
               
               _unitOfWork.Repository<Department>().Add(mappedDep);
                var count = await _unitOfWork.Complete();
                if (count > 0) 
                return RedirectToAction(nameof(Index));
            }

            return View(departmentVM); 
        }
        [HttpGet]

        public async Task <IActionResult> Details(int?id, string viewName= "Details") 
        {

            
            if (!id.HasValue)
                return BadRequest(); 
            var department=await _unitOfWork.Repository<Department>().GetAsync(id.Value);

            var mappedDeps =  _mapper.Map<Department, DepartmentViewModel>(department);


            if (department is null)
                return NotFound(); 

            return View(viewName, mappedDeps);


        }

        [HttpGet]
         public async Task<IActionResult> Edit(int? id)
        {
        
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // to run action only from browser
        public IActionResult Edit([FromRoute] int id,DepartmentViewModel departmentVM)

        {
            if(id!= departmentVM.Id)
                return BadRequest();

            if (!ModelState.IsValid) 
                return View (departmentVM);

            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.Repository<Department>().Update(mappedDep);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating The Department");

                return View(departmentVM);
            }

        }

        [HttpGet]
        public async Task <IActionResult> Delete(int id) 
        {
            return await Details(id, "Delete");
        
        }

        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {

            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.Repository<Department>().Delete(mappedDep);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occured during Updating The Department");

                return View(departmentVM);

            }
        }


    }
}
