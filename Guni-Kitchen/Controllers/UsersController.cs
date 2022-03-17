using Guni_Kitchen.Data;
using Guni_Kitchen.Models;
using Guni_Kitchen.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guni_Kitchen.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UsersController(
            ILogger<UsersController> logger,
            UserManager<MyIdentityUser> userManager,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = from user in _context.Users
                        select user;
            List<UsersListViewModel> viewModelList = new List<UsersListViewModel>();
            foreach (var user in users)
            {
                var viewModel = new UsersListViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    RolesOfUser = await _userManager.GetRolesAsync(user) as List<string>
                };
                viewModelList.Add(viewModel);
            }
            return View(viewModelList);
        }
    }
}
