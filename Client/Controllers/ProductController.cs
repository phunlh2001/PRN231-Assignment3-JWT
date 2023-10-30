using Client.APIs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly HttpClient client;
        private string api;

        public ProductController(IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();

            //var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            //client.DefaultRequestHeaders.Accept.Add(contentType);

            var token = httpContextAccessor.HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            api = "https://localhost:44322/api/products/";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLogin()) return Redirect("/auth/login");

            try
            {
                var p = await client.GetApi<IEnumerable<Product>>(api);
                return View(p);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        [HttpGet("detail/{id}", Name = "detail")]
        public async Task<IActionResult> Details(int id)
        {
            if (!IsLogin()) return Redirect("/auth/login");

            var obj = await client.GetApi<Product>(api + id);
            return View(obj);
        }

        [HttpGet("add", Name = "add")]
        public IActionResult Create()
        {
            if (!IsLogin()) return Redirect("/auth/login");

            return View();
        }

        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage res = await client.PostApi(product, api);
                if (res.IsSuccessStatusCode)
                {
                    return Redirect("/product");
                }
            }
            return View(product);
        }

        [HttpGet("edit/{id}", Name = "edit")]
        public async Task<IActionResult> Update(int id)
        {
            if (!IsLogin()) return Redirect("/auth/login");

            var prod = await client.GetApi<Product>(api + id);
            return View(prod);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage res = await client.PutApi(product, api + id);
                if (res.IsSuccessStatusCode)
                {
                    return Redirect("/product");
                }
            }
            return View();
        }

        [HttpGet("delete/{id}", Name = "delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLogin()) return Redirect("/auth/login");

            var prod = await client.GetApi<Product>(api + id);
            return View(prod);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            HttpResponseMessage res = await client.DeleteAsync(api + id);
            if (res.IsSuccessStatusCode)
            {
                return Redirect("/product");
            }
            var prod = await client.GetApi<Product>(api + id);
            return View(prod);
        }

        private bool IsLogin()
        {
            return HttpContext.Session.GetString("token") != null;
        }
    }
}
