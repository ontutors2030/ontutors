using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public StudentController(UserManager<ApplicationUser> _userManager,
            ApplicationDbContext context)
        {
            this.db = context;
            this.userManager = _userManager;
        }

        [Authorize(Roles = "Student")]        
        public async Task<IActionResult> SearchInTutor()
        {
            ViewBag.SelectedPage = "searchInTutorNavItem";
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);            
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", applicationUser.SpecialtyId);
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", applicationUser.CountryId);
            ViewData["CityId"] = new SelectList(db.City, "Id", "Name", applicationUser.CityId);            
            return View();
        }

        [Authorize(Roles = "Student")]        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchTutor(SearchModel search)
        {
            ViewBag.SelectedPage = "searchInTutorNavItem";
            
            if (ModelState.IsValid)
            {
                var result = userManager.Users.Include(u=>u.Country).Include(u => u.City)
                    .Where(u =>u.UserTypeId=="Tutor" && u.FullName.Contains(search.SearchText))
                    //.Where(u=> search.CountryId==-1 || u.CountryId==search.CountryId)
                    //.Where(u => search.CityId == -1 || u.CityId == search.CityId)
                    .Where(u => search.SpecialtyId == -1 || u.SpecialtyId == search.SpecialtyId)
                    .ToList();
                return View(result);
            }

            ApplicationUser applicationUser = await userManager.GetUserAsync(User);
            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name", search.SpecialtyId);
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name", search.CountryId);
            ViewData["CityId"] = new SelectList(db.City, "Id", "Name", search.CityId);
            return View();
        }
    }
}
