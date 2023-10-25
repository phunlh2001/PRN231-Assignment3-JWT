using Client.APIs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly HttpClient client;
        private string api = "";

        public AuthController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            api = "https://localhost:44322/api/token";
        }

        [HttpGet("login", Name = "login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("userName") != null)
            {
                return Redirect("/product");
            }
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserInfo info)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage res = await client.PostApi(info, api);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    string payload = data.Split(".")[1];
                    string payloadJson = Encoding.UTF8.GetString(Base64UrlEncoder.DecodeBytes(payload));

                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(payloadJson);
                    HttpContext.Session.SetString("userEmail", userInfo.Email);
                    HttpContext.Session.SetString("userName", userInfo.UserName);
                    return Redirect("/product");
                }
                else
                {
                    ViewData["error"] = "Login failed";
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userEmail");
            HttpContext.Session.Remove("userName");
            return Redirect("/product");
        }
    }
}
