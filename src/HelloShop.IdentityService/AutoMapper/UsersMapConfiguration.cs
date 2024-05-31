// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.Models.Users;

namespace HelloShop.IdentityService.AutoMapper;

public class UsersMapConfiguration : Profile
{
    public UsersMapConfiguration()
    {
        CreateMap<UserCreateRequest, User>();
        CreateMap<UserUpdateRequest, User>();
        CreateMap<User, UserDetailsResponse>();
        CreateMap<User, UserListItem>();
    }
}
