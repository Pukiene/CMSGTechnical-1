using System.Collections.Generic;
using System.Linq;
using CMSGTechnical.Domain.Models;
using CMSGTechnical.Mediator.Dtos;
using Xunit;
using BasketEntity = CMSGTechnical.Domain.Models.Basket;
using BasketRequest = CMSGTechnical.Mediator.Basket.GetBasket;

namespace CMSGTechnical.Mediator.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void MenuItem_ToDto_MapsFields_AndChildren()
        {
            // Arrange
            var domain = new MenuItem
            {
                Id = 10,
                Name = "Margherita Pizza",
                Description = "Classic",
                Price = 12.99m,
                Order = 3,
                Category = "Main",
                ChildItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Id = 11,
                        Name = "Extra Cheese",
                        Description = "Addon",
                        Price = 1.50m,
                        Order = 0,
                        Category = "Main",
                        ChildItems = new List<MenuItem>()
                    }
                }
            };

            // Act
            var dto = domain.ToDto();

            // Assert
            Assert.Equal(10, dto.Id);
            Assert.Equal("Margherita Pizza", dto.Name);
            Assert.Equal("Classic", dto.Description);
            Assert.Equal(12.99m, dto.Price);
            Assert.Equal(3, dto.Order);
            Assert.Equal("Main", dto.Category);

            Assert.Single(dto.ChildItems);
            Assert.Equal(11, dto.ChildItems[0].Id);
            Assert.Equal("Extra Cheese", dto.ChildItems[0].Name);
        }

        [Fact]
        public void Basket_ToDto_MapsMenuItems_AndUserId()
        {
            // Arrange
            var basket = new BasketEntity

            {
                Id = 1,
                UserId = 99,
                MenuItems = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Id = 1,
                        Name = "Chocolate Cake",
                        Description = "Rich",
                        Price = 6.99m,
                        Order = 0,
                        Category = "Dessert",
                        ChildItems = new List<MenuItem>()
                    }
                }
            };

            // Act
            var dto = basket.ToDto();

            // Assert
            Assert.Equal(1, dto.Id);
            Assert.Equal(99, dto.UserId);
            Assert.Single(dto.MenuItems);

            var item = dto.MenuItems.First();
            Assert.Equal(1, item.Id);
            Assert.Equal("Chocolate Cake", item.Name);
            Assert.Equal("Dessert", item.Category);
            Assert.Equal(6.99m, item.Price);
        }
    }
}
