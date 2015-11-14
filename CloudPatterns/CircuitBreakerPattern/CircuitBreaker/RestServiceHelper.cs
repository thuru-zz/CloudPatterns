using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace CircuitBreakerPattern.CircuitBreaker
{
    public class RestServiceHelper<T>
    {
        private readonly HttpClient _client;

        public RestServiceHelper(Uri baseAddress)
        {
            _client = new HttpClient();

            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<T>> GetCollectionAsync(string address)
        {
            var response = await _client.GetAsync(address);
                
            List<T> collection = await response.Content.ReadAsAsync<List<T>>();
            return collection;                
        }

        public async Task<List<T>> GetAsync(string address)
        {
            var response = await _client.GetAsync(address);
            
            T item = await response.Content.ReadAsAsync<T>();
            List<T> collection = new List<T>(1) { item };

            return collection;
        }

        public async Task<List<T>> PostAsync(string address, T value)
        {
            var response = await _client.PostAsJsonAsync(address, value);

            T item = await response.Content.ReadAsAsync<T>();

            List<T> collection = new List<T>(1) { item };
            return collection;
        }

        public async Task<List<T>> PutAsync(string addresss, T value)
        {
            var response = await _client.PutAsJsonAsync(addresss, value);

            T item = await response.Content.ReadAsAsync<T>();

            List<T> collection = new List<T>(1) { item };
            return collection;
        }
    }
}