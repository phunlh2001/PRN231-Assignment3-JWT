using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.APIs
{
    public static class ApiHelper
    {
        public static async Task<HttpResponseMessage> PostApi<T>(this HttpClient client, T obj, string api)
        {
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await client.PostAsync(api, content);
            return res;
        }

        public static async Task<T> GetApi<T>(this HttpClient client, string api)
        {
            HttpResponseMessage res = await client.GetAsync(api);

            var data = await res.Content.ReadAsStringAsync();
            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var list = JsonSerializer.Deserialize<T>(data, opt);
            return list ?? default;
        }

        public static async Task<HttpResponseMessage> PutApi<T>(this HttpClient client, T obj, string api)
        {
            string data = JsonSerializer.Serialize(obj);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await client.PutAsync(api, content);
            return res;
        }
    }
}
