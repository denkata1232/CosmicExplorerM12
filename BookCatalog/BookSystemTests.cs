using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using BookCatalogWebApp_M12.Controllers;
using BookCatalogWebApp_M12.Data;
using BookCatalogWebApp_M12.Data.Models;
using BookCatalogWebApp_M12.Models.Book;
using BookCatalogWebApp_M12.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookCatalog.Tests
{
    [TestFixture]
    public class BookSystemTests
    {
        private BookCatalogDbContext _context;
        private string _testUserId = "test-user-id";

        [SetUp]
        public void Setup()
        {
            // Arrange: Конфигуриране на InMemory база с уникално име за всеки тест
            var options = new DbContextOptionsBuilder<BookCatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BookCatalogDbContext(options);

            // Arrange: Подготовка на базови данни
            var category = new Category { Id = 1, Name = "Fiction" };
            _context.Categories.Add(category);

            _context.Books.Add(new Book { Id = 1, Title = "C# Mastery", Description = "Deep dive into C#", CategoryId = 1, OwnerId = _testUserId });
            _context.Books.Add(new Book { Id = 2, Title = "ASP.NET Core Guide", Description = "Web Development", CategoryId = 1, OwnerId = _testUserId });

            _context.SaveChanges();
        }

        private void FakeUserSession(Controller controller)
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, _testUserId),
                new Claim(ClaimTypes.Name, "test@user.com")
            }, "TestAuth");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

      

        [Test]
        public async Task Edit_Get_ValidIdAndOwner_ReturnsView()
        {
            // Arrange
            var controller = new BookController(_context);
            FakeUserSession(controller);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Edit_Get_NonOwner_ReturnsUnauthorized()
        {
            // Arrange
            var controller = new BookController(_context);
            var otherUserIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "other-user") });
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(otherUserIdentity) } };

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedResult>());
        }

        [Test]
        public async Task Delete_Get_ValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = new BookController(_context);
            FakeUserSession(controller);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }


        [Test]
        public async Task Database_AddBook_SuccessfullyIncreasesCount()
        {
            // Arrange
            var newBook = new Book { Id = 3, Title = "SQL basics", Description = "DB fundamentals", CategoryId = 1, OwnerId = "U1" };

            // Act
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            // Assert
            Assert.That(_context.Books.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Database_RemoveBook_SuccessfullyDecreasesCount()
        {
            // Arrange
            var book = await _context.Books.FindAsync(1);

            // Act
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            // Assert
            Assert.That(_context.Books.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Database_UpdateBookTitle_ChangesDataSuccessfully()
        {
            // Arrange
            var book = await _context.Books.FindAsync(1);
            book.Title = "Updated Title";

            // Act
            await _context.SaveChangesAsync();
            var updatedBook = await _context.Books.FindAsync(1);

            // Assert
            Assert.That(updatedBook.Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public void Database_Categories_AreSeededCorrectly()
        {
            // Assert
            Assert.That(_context.Categories.Any(c => c.Name == "Fiction"), Is.True);
        }


        [Test]
        public void BookModel_Properties_SetAndGetCorrectly()
        {
            // Arrange
            var book = new Book { Title = "T", Description = "D" };

            // Assert
            Assert.Multiple(() => {
                Assert.That(book.Title, Is.EqualTo("T"));
                Assert.That(book.Description, Is.EqualTo("D"));
            });
        }

        [Test]
        public void CategoryModel_Name_IsSetCorrectly()
        {
            // Arrange
            var cat = new Category { Name = "Horror" };

            // Assert
            Assert.That(cat.Name, Is.EqualTo("Horror"));
        }

        [Test]
        public void HomeViewModel_CountProperty_Works()
        {
            // Arrange
            var model = new HomeViewModel { AllBooksCount = 50 };

            // Assert
            Assert.That(model.AllBooksCount, Is.EqualTo(50));
        }

        [Test]
        public void BookFormModel_CanStoreData()
        {
            // Arrange
            var model = new BookFormModel { Title = "New Form", CategoryId = 1 };

            // Assert
            Assert.That(model.Title, Is.EqualTo("New Form"));
        }

        [Test]
        public async Task Database_SearchByOwner_ReturnsCorrectBooks()
        {
            // Act
            var books = await _context.Books.Where(b => b.OwnerId == _testUserId).ToListAsync();

            // Assert
            Assert.That(books.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Database_FindNonExistent_ReturnsNull()
        {
            // Act
            var book = await _context.Books.FindAsync(999);

            // Assert
            Assert.That(book, Is.Null);
        }

        [Test]
        public void BookViewModel_HasCorrectData()
        {
            // Arrange
            var model = new BookViewModel { Id = 1, Title = "Model" };

            // Assert
            Assert.That(model.Title, Is.EqualTo("Model"));
        }

        [Test]
        public void Database_Context_IsCorrectType()
        {
            // Assert
            Assert.That(_context.Database.IsInMemory(), Is.True);
        }

        [Test]
        public async Task All_ReturnsEmptyList_WhenNoData()
        {
            // Arrange
            _context.Books.RemoveRange(_context.Books);
            await _context.SaveChangesAsync();
            var controller = new BookController(_context);
            FakeUserSession(controller);

            // Act
            var result = await controller.All() as ViewResult;
            var model = (IEnumerable<BookViewModel>)result.Model;

            // Assert
            Assert.That(model.Count(), Is.EqualTo(0));
        }


        [Test]
        public void BookFormModel_Categories_CanBeAssigned()
        {
            // Arrange
            var model = new BookFormModel { Categories = new List<BookCategoryViewModel>() };

            // Assert
            Assert.That(model.Categories, Is.Not.Null);
        }
    }
}