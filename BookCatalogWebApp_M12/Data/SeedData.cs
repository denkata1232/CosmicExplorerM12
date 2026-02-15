using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookCatalogWebApp_M12.Data.Models;

namespace BookCatalogWebApp_M12.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            var db = provider.GetRequiredService<SpaceObjectCatalogDbContext>();

            db.Database.Migrate();

            var count = await db.Categories.CountAsync();



            if (count == 2)
            {
                var categories = new List<Category>()
                {
                    new Category { Name = "History"},
                     new Category { Name = "Mystery"},
                     new Category { Name = "Romance"},
                     new Category { Name = "Horror"},
                     new Category { Name = "Biography"},
                     new Category { Name = "Sience&Nature"},
                     new Category { Name = "CookBooks"},
                    new Category { Name = "Philosophy"},
                    new Category { Name = "Economics"},
                    new Category { Name = "Travelogue"}
                };

                await db.Categories.AddRangeAsync(categories);
                await db.SaveChangesAsync();

            }

        }
    }

}
