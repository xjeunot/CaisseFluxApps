using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class ClientService : IClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<string> DonneClients()
        {
            var client = _httpClientFactory.CreateClient("ApiClientV1");
            string strUrl = "/api/v1/clients";

            var donneesJson = client.GetStringAsync(strUrl).Result;

            IEnumerable<string> retour = JsonConvert.DeserializeObject<IEnumerable<string>>(donneesJson);
            return retour;
        }

        public ClientItem DonneClient(string id)
        {
            var client = _httpClientFactory.CreateClient("ApiClientV1");
            string strUrl = $"/api/v1/clients/{id}";

            var donneesJson = client.GetStringAsync(strUrl).Result;

            ClientItem retour = JsonConvert.DeserializeObject<ClientItem>(donneesJson);
            return retour;
        }
    }
}
