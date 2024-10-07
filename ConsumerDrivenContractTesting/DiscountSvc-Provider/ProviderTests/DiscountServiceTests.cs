using Microsoft.VisualStudio.TestPlatform.Utilities;
using PactNet;
using Xunit.Abstractions;
using IOutput = PactNet.Infrastructure.Outputters.IOutput;

namespace ProviderTests
{
    public class DiscountServiceTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private bool _disposed;
        private readonly string _serviceUri;

        public DiscountServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _serviceUri = "http://localhost:5195";
        }

        [Fact]
        public void PactWithOrderSvcShouldBeVerified()
        {
            var config = new PactVerifierConfig
            {
                Verbose = true,
                ProviderVersion = "2.0.0",
                CustomHeaders = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json; charset=utf-8"}
                },
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(_output)
                }
            };

            new PactVerifier(config)
                .ServiceProvider("Discounts", _serviceUri)
                .HonoursPactWith("Orders")
                .PactUri(@"c:\temp\pact\OrderSvcConsumer\orders-discounts.json")
                .Verify();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DiscountServiceTests()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
