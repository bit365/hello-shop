// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Grpc.Core;

namespace HelloShop.BasketService.UnitTests.Helpers
{
    public class TestServerCallContext : ServerCallContext
    {
        private readonly Metadata _requestHeaders;

        private readonly CancellationToken _cancellationToken;

        private readonly Metadata _responseTrailers;

        private readonly AuthContext _authContext;

        private readonly Dictionary<object, object> _userState;

        private WriteOptions? _writeOptions;

        public Metadata? ResponseHeaders { get; private set; }

        private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
        {
            _requestHeaders = requestHeaders;
            _cancellationToken = cancellationToken;
            _responseTrailers = [];
            _authContext = new AuthContext(string.Empty, []);
            _userState = [];
        }

        protected override string MethodCore => "MethodName";

        protected override string HostCore => "HostName";

        protected override string PeerCore => "PeerName";

        protected override DateTime DeadlineCore { get; }

        protected override Metadata RequestHeadersCore => _requestHeaders;

        protected override CancellationToken CancellationTokenCore => _cancellationToken;

        protected override Metadata ResponseTrailersCore => _responseTrailers;

        protected override Status StatusCore { get; set; }

        protected override WriteOptions? WriteOptionsCore { get => _writeOptions; set { _writeOptions = value; } }

        protected override AuthContext AuthContextCore => _authContext;

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options) => throw new NotImplementedException();

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            if (ResponseHeaders != null)
            {
                throw new InvalidOperationException("Response headers have already been written.");
            }

            ResponseHeaders = responseHeaders;

            return Task.CompletedTask;
        }

        protected override IDictionary<object, object> UserStateCore => _userState;

        public static TestServerCallContext Create(Metadata? requestHeaders = null, CancellationToken cancellationToken = default) => new(requestHeaders ?? [], cancellationToken);
    }
}
