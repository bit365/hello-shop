// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.OrderingService.Commands.Orders;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Models.Orders;
using HelloShop.OrderingService.Queries;

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

            CreateMap<Order, OrderSummary>().ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus));
            CreateMap<Order, OrderDetails>()
              .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus))
              .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
              .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
              .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
              .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
              .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

            CreateMap<OrderItem, OrderDetailsItem>();
        }
    }
}
