using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SimulateurApps.Evenements;

namespace SimulateurApps.Services
{
    public class ApiConnecteur : IApiConnecteur
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiConnecteur(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async System.Threading.Tasks.Task<bool> EnvoyerEvenementAsync(CaisseEtatEvt caisseEtatEvt)
        {
            var client = _httpClientFactory.CreateClient("ApiSimulationFluxV1");
            string strUrl = "/api/v1/CaisseEtat";

            var clsObjetRequete = new StringContent(JsonConvert.SerializeObject(caisseEtatEvt), Encoding.UTF8, "application/json");

            var reponse = await client.PostAsync(strUrl, clsObjetRequete);

            return reponse.IsSuccessStatusCode;
        }

        public async System.Threading.Tasks.Task<bool> EnvoyerEvenementAsync(CaisseClientEvt caisseClientEvt)
        {
            var client = _httpClientFactory.CreateClient("ApiSimulationFluxV1");
            string strUrl = "/api/v1/CaisseClient";

            var clsObjetRequete = new StringContent(JsonConvert.SerializeObject(caisseClientEvt), Encoding.UTF8, "application/json");

            var reponse = await client.PostAsync(strUrl, clsObjetRequete);

            return reponse.IsSuccessStatusCode;
        }
    }
}
