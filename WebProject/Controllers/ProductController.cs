using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class ProductController : Controller
    {
        Uri baseAddress = new Uri("https://api.escuelajs.co/api/v1");
        private readonly HttpClient _httpClient;
        List<Product> products = new List<Product>();

        public ProductController()
        {
            _httpClient= new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/products?offset=1&limit=12").Result;

            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result; 
                products = JsonConvert.DeserializeObject<List<Product>>(data);
            }

            return View(products);
        }

        public IActionResult ProductDetail()
        {
            return View(products);
        }
    }
}
