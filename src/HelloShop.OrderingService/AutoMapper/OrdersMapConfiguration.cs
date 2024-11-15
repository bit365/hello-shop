﻿// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.OrderingService.Commands.Orders;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Models.Orders;

namespace HelloShop.OrderingService.AutoMapper
{
    public class OrdersMapConfiguration : Profile
    {
        public OrdersMapConfiguration()
        {
            CreateMap<CreateOrderRequest, CreateOrderCommand>().ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));
            CreateMap<BasketItem, CreateOrderCommand.CreateOrderCommandItem>().ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Quantity));
            CreateMap<CreateOrderCommand.CreateOrderCommandItem, OrderItem>();
            CreateMap<CreateOrderCommand, Address>();
        }
    }
}
