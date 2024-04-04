using AutoMapper;
using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.Models.Users;
using Microsoft.Extensions.Options;

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
