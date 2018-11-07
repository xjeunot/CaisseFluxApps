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

        public async Task<IEnumerable<string>> DonneClients()
        {
            var client = _httpClientFactory.CreateClient("ApiClientV1");
            string strUrl = "/api/v1/clients";

            var donneesJson = await client.GetStringAsync(strUrl);

            IEnumerable<string> retour = JsonConvert.DeserializeObject<IEnumerable<string>>(donneesJson);
            return retour;
        }

        public async Task<ClientDTO> DonneClient(string id)
        {
            var client = _httpClientFactory.CreateClient("ApiClientV1");
            string strUrl = $"/api/v1/clients/{id}";

            var donneesJson = await client.GetStringAsync(strUrl);

            ClientDTO retour = JsonConvert.DeserializeObject<ClientDTO>(donneesJson);
            return retour;
        }
    }
}
