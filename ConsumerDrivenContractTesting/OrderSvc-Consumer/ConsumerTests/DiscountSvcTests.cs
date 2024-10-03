using OrderSvc_Consumer;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConsumerTests
{
    public class DiscountSvcTests : IClassFixture<DiscountSvcMock>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _serviceUri;

        public DiscountSvcTests(DiscountSvcMock discountSvcMock)
        {
            _mockProviderService = discountSvcMock.MockProviderService;
            _serviceUri = discountSvcMock.ServiceUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public async Task GetDiscountAdjustmentAmount()
        {
            var discountModel = new DiscountModel { CustomerRating = 4.1 };

            _mockProviderService
                .Given("Rate")
                .UponReceiving("Given a customer rating, an adjustment discount amount will be returned.")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/discount",
                    Body = discountModel,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new DiscountModel
                    {
                        CustomerRating = 4.1,
                        AmountToDiscount = 0.41
                    }
                });

            var httpClient = new HttpClient();
            var response = await httpClient
                .PostAsJsonAsync($"{_serviceUri}/discount", discountModel);
            var discountModelReturned = await response.Content.ReadFromJsonAsync<DiscountModel>();

            Assert.Equal(discountModel.CustomerRating, discountModelReturned.CustomerRating);
        }
    }
}
