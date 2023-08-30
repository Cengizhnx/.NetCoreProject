using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProject.Areas.Identity.Data;
using WebProject.Data;

namespace WebProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AuthDbContext _db;

        public UserController(AuthDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string searchString)
        {
            IEnumerable<ApplicationUser> users = _db.ApplicationUsers;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Email!.Contains(searchString));
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                await _db.ApplicationUsers.AddAsync(user);
                await _db.SaveChangesAsync();
                //TempData["success"] = "User created successfully !";
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser? userFromDb = await _db.ApplicationUsers.FindAsync(id);

            if (userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var model = _db.ApplicationUsers.FirstOrDefault(u => u.Id == user.Id);

                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;

                _db.ApplicationUsers.Update(model);
                await _db.SaveChangesAsync();
                //TempData["success"] = "User updated successfully !";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser? userFromDb = await _db.ApplicationUsers.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteUser(string? id)
        {
            ApplicationUser? user = await _db.ApplicationUsers.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            _db.ApplicationUsers.Remove(user);
            await _db.SaveChangesAsync();
            //TempData["success"] = "User deleted successfully !";
            return RedirectToAction("Index");
        }

    }
}
