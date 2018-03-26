using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace CustomerConsumerPactTests
{
    public class CustomerConsumerTests : IClassFixture<CustomerApiPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public CustomerConsumerTests(CustomerApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public async Task Search_WhenApiFindsCustomer_ReturnsTheCustomer()
        {
            // Arrange

            _mockProviderService
                .Given("There is a customer with an ID of 1")
                .UponReceiving("A GET request to retrieve the customer")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/customer/1",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        id = 1,
                        firstName = "Paul",
                        surname = "Davidson"
                    }
                });

            var consumer = GetCustomerClient();


            // Act

            var customer = await consumer.GetAsync(1);


            // Assert
            Assert.Equal(1, customer.Id);
            Assert.Equal("Paul", customer.FirstName);
            Assert.Equal("Davidson", customer.Surname);

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async Task Search_NoCustomerExists_Returns404()
        {
            // Arrange

            _mockProviderService
                .Given("There is not a customer with an ID of 2")
                .UponReceiving("A GET request to retrieve the customer")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/customer/2",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            var consumer = GetCustomerClient();


            // Act

            var customer = await consumer.GetAsync(2);


            // Assert

            Assert.Null(customer);

            _mockProviderService.VerifyInteractions();
        }

        private ICustomerApiClient GetCustomerClient()
        {
            return new CustomerApiClient(_mockProviderServiceBaseUri);
        }
    }
}
