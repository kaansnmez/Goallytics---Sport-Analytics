
using GoallyticsApp.UI.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoallyticsApp.UI.Controllers
{    
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new UserLoginModel());
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44390/api/Auth/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var tokenModel = JsonSerializer.Deserialize<JwtTokenResponseModel>(responseString, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                    if (tokenModel!=null)
                    {
                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(tokenModel.Token);
                        var claims = token.Claims.ToList();
                        if (tokenModel.Token != null)
                        {
                            claims.Add(new Claim("access_token",tokenModel.Token));
                        }
                        var authProps = new AuthenticationProperties
                        {
                            ExpiresUtc = tokenModel.ExpireDate,
                            IsPersistent = true
                        };
                        var claimsIdentity = new ClaimsIdentity(claims,JwtBearerDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProps);
                        return RedirectToAction("Index","Home");
                    }
                    
                    
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                    return View(model);

                }
            }
            
            return View(model);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout ()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44390/api/User/GetGenderList");
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                var genders = JsonSerializer.Deserialize<List<GenderModel>>(rawJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                return View(new UserRegisterModel
                {
                    Genders = new SelectList(genders, "Id", "Definition")
                });


            }
            //var model = new UserRegisterModel
            //{
            //    Genders = new SelectList(response.Data)
            //}

            return View(new UserRegisterModel());
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            var client = _httpClientFactory.CreateClient();
            var responseSelectList = await client.GetAsync("https://localhost:44390/api/User/GetGenderList");
            if (responseSelectList.IsSuccessStatusCode)
            {
                var rawJson = await responseSelectList.Content.ReadAsStringAsync();
                var genders = JsonSerializer.Deserialize<List<GenderModel>>(rawJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase


                });

                userRegisterModel.Genders = new SelectList(genders, "Id", "Definition", userRegisterModel.GenderId);
                //userRegisterModel.AppRoleId = (int)RoleType.Member;

            }
            if (ModelState.IsValid)
            {
                var userRegisterDto = new RegisterDtoModel
                {
                    FirstName = userRegisterModel.FirstName,
                    LastName = userRegisterModel.LastName,
                    UserName = userRegisterModel.UserName,
                    Email = userRegisterModel.Email,
                    Password = userRegisterModel.Password,
                    DateOfBirth = userRegisterModel.DateOfBirth,
                    AppRoleId = 1,//Member
                    GenderId = userRegisterModel.GenderId,
                };
                var content = new StringContent(JsonSerializer.Serialize(userRegisterDto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44390/api/Auth/Register", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            ModelState.AddModelError(string.Empty, "Kayıt işlemi başarısız oldu.");
            return View(userRegisterModel);



        }
    }
}
