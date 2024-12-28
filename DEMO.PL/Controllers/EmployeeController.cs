using AutoMapper;
using DEMO.BLL.Interfaces;
using DEMO.BLL.Repositories;
using DEMO.DAL.Models;
using DEMO.PL.Helpers;
using DEMO.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DEMO.PL.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
	{

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork , IMapper mapper)
        {
		
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
		{
			IEnumerable<Employee> Employees;
			if (string.IsNullOrEmpty(SearchValue)) 
			{
                 Employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
               
            }
			else
			{
				 Employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(SearchValue);
               
            }

            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
            return View(MappedEmployees);


        }

        public async Task<IActionResult> Details(int? id , string viewName = "Details" )
		{
			if (id == null)
				return BadRequest();
			var Employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);

			if (Employee == null)
				return NotFound();

			var MappedEmployee = _mapper.Map<Employee , EmployeeViewModel> (Employee);
			return View(viewName,MappedEmployee  );

		}
		public IActionResult Create() 
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {

			if (ModelState.IsValid)
			{
                #region // Manual Mapping

                //var MappedEmployee = new Employee()
                //{
                //	Name = employeeVm.Name , 
                //	Age = employeeVm.Age,
                //	Address = employeeVm.Address,
                //	PhoneNumber = employeeVm.PhoneNumber,
                //};
                //Employee employee = (Employee) employeeVm; 
                #endregion
				if(employeeVM.Image is not null)
				{
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                }

                var MappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                 await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
				 await _unitOfWork.CompleteAsync();
				return RedirectToAction(nameof(Index));
			}
			else
			{
                return View(employeeVM);

            }

        }

		public Task<IActionResult> Edit(int? id) 
		{

            return Details(id , "Edit");
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EmployeeViewModel employeeVM , [FromRoute]int id )
		{
			if (id != employeeVM.Id)
				return BadRequest();
			if (!ModelState.IsValid) 
			{
				return View(employeeVM);
			}

			if(employeeVM.Image is not null)
			{
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

            }
            var MappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
			_unitOfWork.EmployeeRepository.Update(MappedEmployee);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));

		}


		public Task<IActionResult> Delete(int? id) 
		{
            return Details(id, "Delete");


        }

        [HttpPost]
		public async Task<IActionResult> Delete(EmployeeViewModel employeeVM , [FromRoute] int id)
		{
			if(id != employeeVM.Id)
				return BadRequest();
			if(!ModelState.IsValid)
				return View(employeeVM);
            var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
            var result = await _unitOfWork.CompleteAsync();
			if (result > 0 && employeeVM.ImageName != null)
				DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
            return RedirectToAction(nameof(Index));
		}



		
    }
}
