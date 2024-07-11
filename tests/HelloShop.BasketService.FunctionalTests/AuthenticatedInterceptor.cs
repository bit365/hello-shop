// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Net.Http.Headers;

namespace HelloShop.BasketService.FunctionalTests
{
    internal class AuthenticatedInterceptor : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsInJvbGVpZCI6IjEiLCJuYmYiOjE3MjA1NzY3NDYsImV4cCI6MTc0MjYwODc0NiwiaWF0IjoxNzIwNTc2NzQ2fQ.ju_D3zeGLKqJYVckbb8Y3yNkp40nOqRAJrdOsISs4d4";

            Metadata headers = [new Metadata.Entry(HeaderNames.Authorization, $"Bearer {token}")];

            var newOptions = context.Options.WithHeaders(headers);

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, newOptions);

            return base.AsyncUnaryCall(request, newContext, continuation);
        }
    }
}
