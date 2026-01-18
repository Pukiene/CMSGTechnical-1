using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using CMSGTechnical.Mediator.Basket;
using CMSGTechnical.Mediator.Dtos;

namespace CMSGTechnical.Code
{
    public sealed class BasketService
    {
        private readonly BasketStorage _storage;

        public event EventHandler<BasketChangedEventArgs>? OnChange;

        public BasketDto Basket { get; }

        public BasketService(BasketDto basket, BasketStorage storage)
        {
            Basket = basket;
            _storage = storage;

            // Cross tab sync, when another tab updates localStorage
            _storage.OnExternalChange += async () => await ReloadFromStorageAsync();
        }

        public async Task InitAsync()
        {
            await _storage.InitCrossTabListenerAsync();
            await ReloadFromStorageAsync();
        }

        public async Task Add(MenuItemDto item)
        {
            var existing = Basket.MenuItems.FirstOrDefault(x => x.Id == item.Id);

            if (existing is null)
            {
                // clone to avoid reusing the same reference
                Basket.MenuItems.Add(new MenuItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Order = item.Order,
                    Category = item.Category,
                    Quantity = 1,
                    ChildItems = item.ChildItems ?? new List<MenuItemDto>()
                });
            }
            else
            {
                existing.Quantity = Math.Max(1, existing.Quantity + 1);
            }

            await PersistAsync();
            RaiseChanged();
        }

        public async Task Remove(MenuItemDto item)
        {
            var existing = Basket.MenuItems.FirstOrDefault(x => x.Id == item.Id);
            if (existing is null) return;

            existing.Quantity = Math.Max(0, existing.Quantity - 1);

            if (existing.Quantity == 0)
            {
                Basket.MenuItems.Remove(existing);
            }

            await PersistAsync();
            RaiseChanged();
        }

        private async Task ReloadFromStorageAsync()
        {
            var lines = await _storage.LoadAsync(); // List<BasketLinePersist>

            // Rebuild basket quantities using existing menu items as the catalog source
            var byId = Basket.MenuItems.ToDictionary(x => x.Id, x => x);

            var newList = new List<MenuItemDto>();

            foreach (var line in lines)
            {
                if (byId.TryGetValue(line.Id, out var existing))
                {
                    existing.Quantity = Math.Max(1, line.Quantity);
                    newList.Add(existing);
                }
            }

            Basket.MenuItems.Clear();

            // IMPORTANT, no AddRange here, fixes CS0411
            foreach (var x in newList)
            {
                Basket.MenuItems.Add(x);
            }

            RaiseChanged();
        }

        private async Task PersistAsync()
        {
            var lines = Basket.MenuItems
                .Select(x => new BasketLinePersist
                {
                    Id = x.Id,
                    Quantity = Math.Max(1, x.Quantity)
                })
                .ToList();

            await _storage.SaveAsync(lines);
        }

        private void RaiseChanged()
        {
            OnChange?.Invoke(this, new BasketChangedEventArgs { Basket = Basket });
        }
    }
}