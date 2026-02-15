using BookCatalogWebApp_M12.Data;
using BookCatalogWebApp_M12.Data.Models;
using BookCatalogWebApp_M12.Models.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookCatalogWebApp_M12.Controllers
{
    [Authorize]
    public class SpaceObjectController : Controller
    {

        private readonly SpaceObjectCatalogDbContext data;
        public SpaceObjectController(SpaceObjectCatalogDbContext _context)
        {
            data = _context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var spaceObj = await data.SpaceObjects
               .Select(b => new SpaceObjectViewModel
               {
                   Id = b.Id,
                   Name = b.Name,
                   Category = b.Category.Name,
                   Owner = b.Owner.UserName,
                   OwnerId = b.OwnerId

               })
                .ToListAsync();

            return View(spaceObj);

        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var book = await data.SpaceObjects
                .Where(b => b.Id == id)
                .Select(b => new SpaceObjectViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Category = b.Category.Name,
                    CreatedOn = b.CreatedOn.ToString("dd/MM/yyyy"),
                    Owner = b.Owner.UserName ,
                    OwnerId = b.OwnerId,
                    Founders = b.spaceObjectsFounders.Select(ba => ba.Founder.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (book == null) return BadRequest();

            return View(book);
        }
        public async Task<IActionResult> Create()
        {
            var model = new spaceObjectFormModel
            {
                Categories = await GetCategories()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(spaceObjectFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(model);
            }

            var spaceObject = new spaceObject
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                CreatedOn = DateTime.Now,
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var founder = await data.Founders.FirstOrDefaultAsync(a => a.Name == model.FounderName);
            if (founder == null)
            {
                founder = new Founder { Name = model.FounderName };
                await data.Founders.AddAsync(founder);
            }

            await data.SpaceObjects.AddAsync(spaceObject);
            await data.SaveChangesAsync();

            await data.spaceObjectsFounder.AddAsync(new spaceObjectFounder { spaceObjectId = spaceObject.Id, FounderId = founder.Id });
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var spaceObject = await data.SpaceObjects
        .Include(b => b.spaceObjectsFounders)
        .ThenInclude(ba => ba.Founder)
        .FirstOrDefaultAsync(b => b.Id == id);

            if (spaceObject == null || (spaceObject.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)
                && User.Identity.Name != "admin@test.bg"))
            { return Unauthorized(); }


            var model = new spaceObjectFormModel
            {
                Name = spaceObject.Name,
                Description = spaceObject.Description,
                CategoryId = spaceObject.CategoryId,
                FounderName = spaceObject.spaceObjectsFounders.Select(ba => ba.Founder.Name).FirstOrDefault() ?? "",
                Categories = await GetCategories()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, spaceObjectFormModel model)
        {
            var spaceObject = await data.SpaceObjects.FindAsync(id);
            if (spaceObject == null || (spaceObject.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)
                && User.Identity.Name != "admin@test.bg"))
                return Unauthorized();


            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(model);
            }

            spaceObject.Name = model.Name;
            spaceObject.Description = model.Description;
            spaceObject.CategoryId = model.CategoryId;
            var currentLinks = data.spaceObjectsFounder.Where(ba => ba.spaceObjectId == id);
            data.spaceObjectsFounder.RemoveRange(currentLinks);

            var founder = await data.Founders.FirstOrDefaultAsync(a => a.Name == model.FounderName);
            if (founder == null)
            {
                founder = new Founder { Name = model.FounderName };
                await data.Founders.AddAsync(founder);
                await data.SaveChangesAsync();
            }

            await data.spaceObjectsFounder.AddAsync(new spaceObjectFounder { spaceObjectId = id, FounderId = founder.Id });

            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var spaceObject = await data.SpaceObjects
                .Where(b => b.Id == id)
                .Select(b => new SpaceObjectViewModel { Id = b.Id, Name = b.Name, OwnerId = b.OwnerId })
                .FirstOrDefaultAsync();

            if (spaceObject == null || (spaceObject.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier) && User.Identity.Name != "admin@test.bg"))
            {
                return Unauthorized();
            }
            return View(spaceObject);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spaceObject = await data.SpaceObjects.FindAsync(id);
            if (spaceObject == null || (spaceObject.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier) && User.Identity.Name != "admin@test.bg"))
            {
                return Unauthorized();
            }
            data.SpaceObjects.Remove(spaceObject);
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        private async Task<IEnumerable<spaceObjectCategoryViewModel>> GetCategories()
        {
            return await data.Categories
                .Select(c => new spaceObjectCategoryViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }
    }
}
