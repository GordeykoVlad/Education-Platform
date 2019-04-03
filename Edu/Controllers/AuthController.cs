namespace Edu.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using InputModels.Auth;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Repositories;
    using ViewModels.Auth;

    public class AuthController : Controller
    {
        private readonly UserRepository _userRepository;
        
        public AuthController()
        {
            _userRepository = new UserRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("SignIn", "Auth");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Exam");
            }
            return View(new SignInModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInData data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Login) || string.IsNullOrWhiteSpace(data.Password))
                {
                    return View(new SignInModel {Message = "Не все поля заполнены"});
                }

                data.Login = data.Login.Trim().ToLower();
                var user = await _userRepository.GetByLogin(data.Login);
                if (user == null)
                {
                    return View(new SignInModel {Message = "Пользователь с таким логином и паролем не найден"});
                }

                string passhash;
                using(var sha256 = SHA256.Create())  
                {  
                    var passwordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data.Password));    
                    passhash = BitConverter.ToString(passwordBytes).Replace("-", "").ToLower();
                }
                if (passhash != user.Password)
                {
                    return View(new SignInModel {Message = "Пользователь с таким логином и паролем не найден"});
                }

                await Authenticate(user.Id);
                return RedirectToAction("Language", "Exam");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(new SignInModel {Message = "Ошибка при аутентификации пользователя"});
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Language", "Exam");
            }
            return View(new RegisterModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterData data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data.Login) || string.IsNullOrWhiteSpace(data.Password))
                {
                    return View(new RegisterModel { Message = "Не все поля заполнены" });
                }

                data.Login = data.Login.Trim().ToLower();
            
                if (data.Password != data.PasswordConfirm)
                {
                    return View(new RegisterModel { Message = "Пароли не совпадают" });
                }
                
                if (await _userRepository.GetByLogin(data.Login) != null)
                {
                    return View(new RegisterModel { Message = "Такой пользователь уже существует" });
                }

                string passhash;
                using(var sha256 = SHA256.Create())  
                {  
                    var passwordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data.Password));    
                    passhash = BitConverter.ToString(passwordBytes).Replace("-", "").ToLower();
                }
                var newUser = new User
                {
                    Login = data.Login,
                    Password= passhash
                };
                
                newUser = await _userRepository.Add(newUser);
                
                if (newUser == null)
                {
                    return View(new RegisterModel { Message = "Ошибка при регистрации пользователя" });
                }

                await Authenticate(newUser.Id);
                return RedirectToAction("Language", "Exam");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View(new RegisterModel { Message = "Ошибка при регистрации пользователя" });
            }
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Language", "Exam");
        }
        
        private async Task Authenticate(long id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, id.ToString()),
            };
            
            var ci = new ClaimsIdentity(
                claims, 
                "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);
            
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(ci));
        }
    }
}