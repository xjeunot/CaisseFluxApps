using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.API.Bdd.Interfaces
{
    public interface IClientsService
    {
        Task<IEnumerable<Models.ClientItem>> DonneClients();

        Task<Models.ClientItem> DonneClient(string id);

        Task AjouterClient(Models.ClientItem model);

        /*public async Task<bool> MajClient(Models.Client modele);*/

        Task<DeleteResult> DetruireClient(string id);

        Task<DeleteResult> DetruireTousClients();
    }
}
