// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Grpc.Core;
using HelloShop.BasketService.Protos;

namespace HelloShop.BasketService.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
