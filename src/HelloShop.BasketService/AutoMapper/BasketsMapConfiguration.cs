// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.BasketService.Entities;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.AutoMapper
{
    public class BasketsMapConfiguration : Profile
    {
        public BasketsMapConfiguration()
        {
            CreateMap<CustomerBasket, CustomerBasketResponse>();
            CreateMap<BasketItem, BasketListItem>().ReverseMap();
            CreateMap<UpdateBasketRequest, CustomerBasket>();
        }
    }
}
