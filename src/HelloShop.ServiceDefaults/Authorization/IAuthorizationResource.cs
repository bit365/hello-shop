using HelloShop.ServiceDefaults.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace HelloShop.ServiceDefaults.Authorization;

public interface IAuthorizationResource
{
    string ResourceType => GetType().Name;

    string ResourceId => GetType().GetProperty(EntityConnstants.DefaultKey)?.GetValue(this)?.ToString() ?? throw new NotImplementedException();
}