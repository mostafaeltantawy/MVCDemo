using DEMO.BLL.Interfaces;
using DEMO.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DEMO.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            var Departments =await  _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(Departments);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            TempData["Message"] = "Department Is CreatedSuccessfully"; 
            if (ModelState.IsValid)
            {
                _unitOfWork.DepartmentRepository.AddAsync(department);
                var result =await _unitOfWork.CompleteAsync();
                if (result > 0)
                    TempData["Message"] = "Department Has Been created";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public async Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if (id == null)
            {
                return BadRequest(); // Status Code 400
            }

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if (department == null)
                return NotFound();

            return View(ViewName,department);
        }

        [HttpGet]
        public Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //    return BadRequest();
            //var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            //if (department == null)
            //    return NotFound();
            //return View(department);
            return Details(id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department , [FromRoute] int id )
        {
            if (id != department.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log Exception
                    // 2 . Form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);

        }


        public Task<IActionResult> Delete(int? id) 
        {
            return Details(id, "Delete");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id) 
        {
            if (id != department.Id)
                return BadRequest();

            try
            {
				_unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// 1. Log Exception
				// 2 . Form
				ModelState.AddModelError(string.Empty, ex.Message);
                return View (department);   
			}


		}

    }
}
