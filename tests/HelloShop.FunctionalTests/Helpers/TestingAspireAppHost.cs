// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Aspire.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace HelloShop.FunctionalTests.Helpers
{
    public class TestingAspireAppHost : DistributedApplicationFactory
    {
        private readonly TaskCompletionSource<IServiceProvider> _serviceTcs = new();

        public TestingAspireAppHost() : base(typeof(Projects.HelloShop_AppHost)) { }

        protected override void OnBuilderCreated(DistributedApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });
        }

        protected override void OnBuilt(DistributedApplication application)
        {
            _serviceTcs.SetResult(application.Services);
        }

        public IServiceProvider Services => _serviceTcs.Task.GetAwaiter().GetResult();
    }

}
