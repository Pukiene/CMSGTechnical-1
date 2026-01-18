//MenuItemDto.cs

using CMSGTechnical.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMSGTechnical.Mediator.Dtos
{
    public class MenuItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Order { get; set; } = 0;

        // Used when item is in the basket, menu items from catalog can keep it 0
        public int Quantity { get; set; } = 0;

        // New field for UI grouping
        public string Category { get; set; } = "Main";

        public List<MenuItemDto> ChildItems { get; set; } = new();
    }

    public static class MenuItemExtensions
    {
        public static IEnumerable<MenuItemDto> ToDto(this IEnumerable<MenuItem> items) =>
            items.Select(i => i.ToDto());

        public static MenuItemDto ToDto(this MenuItem menuItem)
        {
            return new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Order = menuItem.Order,
                Category = menuItem.Category,

                // If domain model does not have Category yet, keep default here
               // Category = "Main",

                ChildItems = menuItem.ChildItems.ToDto().ToList()
            };
        }
    }
}

