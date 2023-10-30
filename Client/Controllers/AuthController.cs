using Client.APIs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Models;
using System.Net.Http;
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
            api = "https://localhost:44322/api/token";
        }

        [HttpGet("login", Name = "login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("token") != null)
                return Redirect("/product");

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
                    var result = await res.Content.ReadAsStringAsync();
                    string token = result.Trim('"');

                    HttpContext.Session.SetString("token", token);
                    string payload = token.Split(".")[1];
                    string payloadJson = Encoding.UTF8.GetString(Base64UrlEncoder.DecodeBytes(payload));
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(payloadJson);

                    HttpContext.Session.SetString("userEmail", userInfo.Email);
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
            HttpContext.Session.Remove("token");
            return Redirect("/product");
        }
    }
}
