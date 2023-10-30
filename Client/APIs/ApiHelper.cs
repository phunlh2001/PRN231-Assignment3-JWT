using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.APIs
{
    public static class ApiHelper
    {
        public static async Task<HttpResponseMessage> PostApi<T>(this HttpClient client, T obj, string api)
        {
            var data = JsonConvert.SerializeObject(obj);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await client.PostAsync(api, content);
            return res;
        }

        public static async Task<T> GetApi<T>(this HttpClient client, string api)
        {
            string data = await client.GetStringAsync(api);
            var res = JsonConvert.DeserializeObject<T>(data);

            return res ?? default;
        }

        public static async Task<HttpResponseMessage> PutApi<T>(this HttpClient client, T obj, string api)
        {
            var data = JsonConvert.SerializeObject(obj);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await client.PutAsync(api, content);
            return res;
        }
    }
}
