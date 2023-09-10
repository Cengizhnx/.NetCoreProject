using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Areas.Identity.Data;
using WebProject.Data;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly AuthDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(AuthDbContext db, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            string userId = GetUserId();
            List<Product> products = GetProduct().ToList();

            if (ModelState.IsValid)
            {
                var orderItem = new Order()
                {
                    UserId = userId,
                    Product = "ss",
                    Total = GetTotal(),
                };
                await _db.Orders.AddAsync(orderItem);
                await _db.SaveChangesAsync();
                //TempData["success"] = "User created successfully !";
                return RedirectToAction("Index");
            }
            return View();
        }
        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        private IEnumerable<Cart> GetCart(string id)
        {
            var carts = _db.Carts.Where(i => i.UserId == id);
            return carts;
        }
        private IEnumerable<Product> GetProduct()
        {
            var products = _db.Products;
            return products;
        }
        private int GetTotal()
        {
            var total = 0;
            string userId = GetUserId();
            var carts = GetCart(userId);
            var products = GetProduct();

            foreach (var cart in carts)
            {
                foreach (var product in products)
                {
                    if (product.Id == cart.ProductId)
                    {
                        total += product.Price;
                    }
                }
            }
            return total;
        }
    }
}
