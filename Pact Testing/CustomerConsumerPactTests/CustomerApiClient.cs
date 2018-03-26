using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomerConsumerPactTests
{
    public class CustomerApiClient : ICustomerApiClient
    {
        private readonly HttpClient _httpClient;

        public CustomerApiClient(string baseUri = "")
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Customer> GetAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"customer/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Customer>(responseAsString);
                }

                throw new Exception($"Calling customer API failed with status code [{response.StatusCode}], reason [{response.ReasonPhrase}]");
            }
            catch
            {
                return await Task.FromResult(default(Customer));
            }
        }
    }

    public interface ICustomerApiClient
    {
        Task<Customer> GetAsync(int customerId);
    }
}
