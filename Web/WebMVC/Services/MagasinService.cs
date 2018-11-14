using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class MagasinService : IMagasinService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MagasinService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IEnumerable<CaissePhotoSimple> DonneCaissesPhotoSimples()
        {
            var client = _httpClientFactory.CreateClient("ApiMagasinV1");
            string strUrl = "/api/v1/caissephoto";

            var donneesJson = client.GetStringAsync(strUrl).Result;

            IEnumerable<CaissePhotoSimple> retour = JsonConvert.DeserializeObject<IEnumerable<CaissePhotoSimple>>(donneesJson);
            return retour;
        }
    }
}
