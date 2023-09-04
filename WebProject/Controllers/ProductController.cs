using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using WebProject.Areas.Identity.Data;
using WebProject.Data;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AuthDbContext _db;

        public ProductController(AuthDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string searchString)
        {
            IEnumerable<Product> products = _db.Products;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Title!.Contains(searchString));
            }

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? products = await _db.Products.FindAsync(id);

            if (products.Status == true)
            {
                return View(products);
            }
            else
            {
                return RedirectToAction("Index");
            }

            if (products == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                //TempData["success"] = "User created successfully !";
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? products = await _db.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                //TempData["success"] = "User updated successfully !";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product? product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, Authorize(Roles = "Admin"), ActionName("Delete")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            Product? product = await _db.Products.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            //TempData["success"] = "User deleted successfully !";
            return RedirectToAction("Index");
        }

    }
}
