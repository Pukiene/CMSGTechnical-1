using System;
using CMSGTechnical.Mediator.Dtos;

namespace CMSGTechnical.Code
{
    public sealed class BasketChangedEventArgs : EventArgs
    {
        public BasketDto Basket { get; init; } = default!;
    }
}

