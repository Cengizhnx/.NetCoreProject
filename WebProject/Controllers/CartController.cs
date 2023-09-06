﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Net;
using WebProject.Areas.Identity.Data;
using WebProject.Data;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class CartController : Controller
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(AuthDbContext db, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            string userId = GetUserId();
            IEnumerable<Cart> carts = _db.Carts;
            carts = carts.Where(i => i.UserId == userId);
            return View(carts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            string userId = GetUserId(); 
            var carts = _db.Carts.Where(i => i.UserId == userId);

            if (ModelState.IsValid)
            {
                var product = await _db.Products.FindAsync(id);
                if (carts.All(i => i.ProductId != id))
                {
                    var cartItem = new Cart()
                    {
                        UserId = userId,
                        ProductId = id,
                        Total = product.Price,
                    };
                    await _db.Carts.AddAsync(cartItem);
                    await _db.SaveChangesAsync();
                    //TempData["success"] = "User created successfully !";
                    return RedirectToAction("Index");
                }

            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Cart? cart = await _db.Carts.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            _db.Carts.Remove(cart);
            await _db.SaveChangesAsync();
            //TempData["success"] = "User deleted successfully !";
            return RedirectToAction("Index");
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

    }
}
