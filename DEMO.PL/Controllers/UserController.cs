using AutoMapper;
using DEMO.DAL.Models;
using DEMO.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEMO.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    Fname = U.Fname,
                    Lname = U.Lname,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result,
                }).ToListAsync();
                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(SearchValue);
                if(User != null)
                {
                    var MappedUser = new UserViewModel()
                    {
                        Id = User.Id,
                        Fname = User.Fname,
                        Lname = User.Lname,
                        Email = User.Email,
                        PhoneNumber = User.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(User).Result,
                    };
                    return View(new List<UserViewModel>() { MappedUser });
                }
                return View(new List<UserViewModel>()); 
         
            }

        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();

            var User = await _userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();

            var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);
            return View(ViewName, MappedUser);

        }

        public Task<IActionResult> Edit(string id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {

                    var User = await _userManager.FindByIdAsync(id);
                    User.PhoneNumber = model.PhoneNumber;
                    User.Fname = model.Fname;
                    User.Lname = model.Lname;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        public Task<IActionResult> Delete(string id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string email, [FromRoute] string id)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return NotFound();
            if (User.Id != id)
                return BadRequest();
            await _userManager.DeleteAsync(User);
            return RedirectToAction(nameof(Index));
            
        }


    }
}
