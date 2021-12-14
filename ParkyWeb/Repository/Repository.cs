using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;
        public Repository(IHttpClientFactory _clientFactory)
        {
            this._clientFactory = _clientFactory;
        }
        public async Task<bool> CreateAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (entity != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response =await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }

            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (entity != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
