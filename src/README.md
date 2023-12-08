## HelloShop.IdentityService

身份认证微服务，提供用户注册，登录，登出，修改密码，修改用户信息等接口。

## HelloShop.ProductService

产品微服务，提供产品目录，产品列表和产品详情等接口。

## HelloShop.BasketService

购物车微服务，提供购物车的增删改查等接口。

## HelloShop.OrderingService

订单微服务，提供订单的增删改查等接口。

## HelloShop.ApiService

微服务聚合网关，用于聚合所有微服务，提供统一的入口，将多个微服务聚合成一个微服务，对外提供统一的接口。


## HelloShop.ServiceDefaults

一个 .NET Aspire 共享项目，用于管理在解决方案中与复原能力、服务发现和遥测相关的项目中重复使用的配置。

## HelloShop.AppHost

Aspire 主机，用于演示如何使用 Aspire 构建微服务，一个业务流程协调程序项目，旨在连接和配置应用的不同项目和服务，业务流程协调程序应设置为启动项目


## HelloShop.WebApp

基于 ASP.NET Core Blazor 的应用项目, 用于演示如何使用 Blazor 开发 Web 应用。

## HelloShop.HybridApp

混合应用，用于演示如何使用 Blazor 开发混合应用，包括桌面应用，安卓应用和 IOS 应用，需要 Visual Studio 2022 Preview 并安装 MAUI 负载模块。 Visual Studio 2022 安装 MAUI 负载模块。

## HelloShop.sln

此解决方案包括所有以上项目，需要机器安装安装 MAUI 负载模块。

## HelloShop.Web.sln

此解决方案包括 除 HelloShop.HybridApp 外的项目，如果机器未安装或不需要 MAUI 可以使用此解决方案。
