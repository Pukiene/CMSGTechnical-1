using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMSGTechnical.Domain.Interfaces;
using CMSGTechnical.Mediator.Dtos;
using MediatR;

namespace CMSGTechnical.Mediator.Basket
{
    public record GetBasket(int Id) : IRequest<BasketDto>;

    public class GetBasketHandler : IRequestHandler<GetBasket, BasketDto>
    {

        private IRepo<Domain.Models.Basket> Baskets { get; }

        public GetBasketHandler(IRepo<Domain.Models.Basket> baskets)
        {
            Baskets = baskets;
        }

        public async Task<BasketDto> Handle(GetBasket request, CancellationToken cancellationToken)
        {
            var r = await Baskets.Get(request.Id, cancellationToken);
            if (r is null)
            {
                // simplest behaviour for now
                return new BasketDto
                {
                    Id = request.Id,
                    MenuItems = new List<MenuItemDto>()
                };
            }

            return r.ToDto();

        }
    }
}
