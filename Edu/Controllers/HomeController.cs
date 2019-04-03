using Microsoft.AspNetCore.Mvc;

namespace Edu.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Repositories;
    using ViewModels.Home;

    public class HomeController : Controller
    {
        private readonly UserRepository _userRepository;
        
        public HomeController()
        {
            _userRepository = new UserRepository();
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            //return View();
            var model = new IndexModel();

            if (!User.Identity.IsAuthenticated) 
                return View(model);
            
            var user = await _userRepository.Get(Convert.ToInt64(User.Identity.Name));
            model.UserName = user.Login;
            return View(model);
        }
    }
}