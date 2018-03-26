using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace CustomerConsumerPactTests
{
    public class CustomerApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort => 9222;
        public string MockProviderServiceBaseUri => string.Format("http://localhost:{0}", MockServerPort);

        public CustomerApiPact()
        {
            PactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\pacts",
                LogDir = @".\pact_logs"
            });

            PactBuilder
                .ServiceConsumer("CUSTOMER CONSUMER")
                .HasPactWith("CUSTOMER PROVIDER");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        private void CreatePactFile()
        {
            PactBuilder.Build();
        }

        //private void PublishPactFileToBroker()
        //{
        //    bool.TryParse(Environment.GetEnvironmentVariable("CUSTOMER_PUBLISH_PACT"), out var publishPact);
        //    var buildNumber = Environment.GetEnvironmentVariable("CUSTOMER_BUILD_NUMBER");

        //    if (publishPact && !string.IsNullOrEmpty(buildNumber))
        //    {
        //        var pactPublisher = new PactPublisher("http://pact-broker.test.com/");

        //        pactPublisher.PublishToBroker(
        //            @"..\..\..\pacts\customer-customer.json",
        //            buildNumber,
        //            new[] { "master" });
        //    }
        //}

        #region IDisposable Support

        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    CreatePactFile();
                    //PublishPactFileToBroker();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}
