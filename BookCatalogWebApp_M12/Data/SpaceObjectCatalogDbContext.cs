using BookCatalogWebApp_M12.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogWebApp_M12.Data
{
    public class SpaceObjectCatalogDbContext : IdentityDbContext
    {
        //- admin
        // admin@test.bg 
        //Admin1
        //- user

        //test@test.com
        //test123

        public SpaceObjectCatalogDbContext(DbContextOptions<SpaceObjectCatalogDbContext> options)
            : base(options)
        {
        }
        public DbSet<spaceObject> SpaceObjects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Founder> Founders { get; set; }
        public DbSet<spaceObjectFounder> spaceObjectsFounder { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<spaceObjectFounder>()
                .HasKey(ba => new { ba.spaceObjectId, ba.FounderId });

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Star" },
                new Category { Id = 2, Name = "Planet" }
                
            );
      

            base.OnModelCreating(builder);
        }
    }
}
