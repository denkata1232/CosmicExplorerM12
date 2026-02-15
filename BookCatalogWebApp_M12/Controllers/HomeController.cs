using BookCatalogWebApp_M12.Data;
using BookCatalogWebApp_M12.Models;
using BookCatalogWebApp_M12.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BookCatalogWebApp_M12.Controllers
{
    public class HomeController : Controller
    {

        private readonly SpaceObjectCatalogDbContext data;
        public HomeController(SpaceObjectCatalogDbContext _data)
        {
            data = _data;
        }
        public async Task<IActionResult> Index()
        {
            var categoriesWithCount = await data.Categories
                .Select(c => new HomeCategoryViewModel
                {
                    CategoryName = c.Name,
                    spaceObjectsCount = c.spaceObjects.Count()
                })
                .ToListAsync();

            var userSpaceObjectsCount = -1;

            if (User.Identity?.IsAuthenticated ?? false)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                userSpaceObjectsCount = await data.SpaceObjects.CountAsync(b => b.OwnerId == userId);
            }

            var model = new HomeViewModel
            {
                AllspaceObjectsCount = await data.SpaceObjects.CountAsync(),
                UserspaceObjectsCount = userSpaceObjectsCount,
                CategoriesWithBooksCount = categoriesWithCount
            };

            return View(model);
        }
    }
}
