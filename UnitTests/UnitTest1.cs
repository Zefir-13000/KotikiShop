using KotikiShop.DataAccess.Data;
using KotikiShop.DataAccess.Repository;
using KotikiShop.DataAccess.Repository.IRepository;
using KotikiShop.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class CatRepositoryTests
    {
        private ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new ApplicationDbContext(options);
            return context;
        }

        [Fact]
        public void UpdateCat_ShouldUpdateCat()
        {
            // Arrange
            var dbContext = GetInMemoryContext();

            var catRepo = new CatRepository(dbContext);

            // Add a cat to the in-memory database
            var cat = new Cat
            {
                Id = 1,
                Name = "Tommy",
                Description = "A cute cat",
                Price = 20.5f,
                Birthday = new DateOnly(2020, 1, 1),
                Gender = CatGender.MALE
            };
            dbContext.Cats.Add(cat);
            dbContext.SaveChanges();

            // Act
            var catToUpdate = dbContext.Cats.First();
            catToUpdate.Name = "Tommy Updated";
            catToUpdate.Price = 25.0f;

            ((ICatRepository)catRepo).Update(catToUpdate);
            dbContext.SaveChanges();

            // Assert
            var updatedCat = dbContext.Cats.First();
            Assert.Equal("Tommy Updated", updatedCat.Name);
            Assert.Equal(25.0f, updatedCat.Price);
        }

        [Fact]
        public void UpdateCart_ShouldUpdateCartItems()
        {
            // Arrange
            var dbContext = GetInMemoryContext();
            var cartRepo = new CartRepository(dbContext);

            var cart = new Cart
            {
                ApplicationUserId = "user123",
                CartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, UnitPrice = 50.0f }
            }
            };
            dbContext.Carts.Add(cart);
            dbContext.SaveChanges();

            // Act
            var cartToUpdate = dbContext.Carts.Include(c => c.CartItems).First();
            cartToUpdate.CartItems.First().Quantity = 5; // Update quantity
            ((ICartRepository)cartRepo).Update(cartToUpdate);
            dbContext.SaveChanges();

            // Assert
            var updatedCart = dbContext.Carts.Include(c => c.CartItems).First();
            Assert.Equal(5, updatedCart.CartItems.First().Quantity);  // Ensure quantity is updated
        }

        [Fact]
        public void UpdateCatFamily_ShouldUpdateName()
        {
            // Arrange
            var dbContext = GetInMemoryContext();
            var catFamilyRepo = new CatFamilyRepository(dbContext);

            var catFamily = new CatFamily { Name = "Old Family" };
            dbContext.CatFamilies.Add(catFamily);
            dbContext.SaveChanges();

            // Act
            var catFamilyToUpdate = dbContext.CatFamilies.First();
            catFamilyToUpdate.Name = "New Family";  // Update the name
            ((ICatFamilyRepository)catFamilyRepo).Update(catFamilyToUpdate);
            dbContext.SaveChanges();

            // Assert
            var updatedCatFamily = dbContext.CatFamilies.First();
            Assert.Equal("New Family", updatedCatFamily.Name);  // Ensure name is updated
        }

        [Fact]
        public void UpdateCatLike_ShouldUpdateUserId()
        {
            // Arrange
            var dbContext = GetInMemoryContext();
            var catLikesRepo = new CatLikesRepository(dbContext);

            var catLike = new CatLike
            {
                CatId = 1,
                UserId = "user123"
            };
            dbContext.CatLikes.Add(catLike);
            dbContext.SaveChanges();

            // Act
            var catLikeToUpdate = dbContext.CatLikes.First();
            catLikeToUpdate.UserId = "user456";  // Update UserId
            ((ICatLikesRepository)catLikesRepo).Update(catLikeToUpdate);
            dbContext.SaveChanges();

            // Assert
            var updatedCatLike = dbContext.CatLikes.First();
            Assert.Equal("user456", updatedCatLike.UserId);  // Ensure UserId is updated
        }
    }
}
