using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Mocks.MockHttpService;
using System;

namespace ConsumerTests
{
    public class DiscountSvcMock : IDisposable
    {
        private readonly IPactBuilder _pactBuilder;
        private readonly int _servicePort = 9222;
        private bool _disposed = false;

        public IMockProviderService MockProviderService { get; }
        public string ServiceUri => $"http://localhost:{_servicePort}";


        public DiscountSvcMock()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"c:\temp\pact\OrderSvcConsumer",
                LogDir = @"c:\temp\pact\OrderSvcConsumer\logs"
            };

            _pactBuilder = new PactBuilder(pactConfig)
                .ServiceConsumer("Orders")
                .HasPactWith("Discounts");

            MockProviderService = _pactBuilder.MockService(_servicePort,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _pactBuilder.Build();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}