using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.API.Bdd.Interfaces
{
    public interface IClientsService
    {
        Task<IEnumerable<Models.ClientItem>> DonneClients();

        Task<Models.ClientItem> DonneClient(string id);

        Task<Models.ClientItem> RechercherClientUniqueAvecNom(string nom);

        void AjouterClient(Models.ClientItem model);

        Task<bool> MajClient(Models.ClientItem model);

        Task<DeleteResult> DetruireClient(string id);

        Task<DeleteResult> DetruireTousClients();
    }
}
