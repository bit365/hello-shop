// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.OrderingService.Commands.Orders;
using HelloShop.OrderingService.Models.Orders;

namespace HelloShop.OrderingService.AutoMapper
{
    public class OrdersMapConfiguration : Profile
    {
        public OrdersMapConfiguration()
        {
            CreateMap<BasketItem, CreateOrderCommand.CreateOrderCommandItem>().ForMember(dest => dest.Units, opts => opts.MapFrom(src => src.Quantity));
            CreateMap<CreateOrderRequest, CreateOrderCommand>().ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));
        }
    }
}
