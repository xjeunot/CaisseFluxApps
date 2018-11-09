using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magasin.API.Bdd.Interfaces
{
    public interface ICaissesService
    {
        Task<IEnumerable<Models.CaissesItem>> DonneCaisses();

        Task<Models.CaissesItem> DonneCaisse(string id);

        Task<Models.CaissesItem> RechercherCaisseUniqueAvecNom(string nom);

        void AjouterCaisse(Models.CaissesItem model);

        Task<bool> MajCaisse(Models.CaissesItem model);

        Task<DeleteResult> DetruireCaisse(string id);

        Task<DeleteResult> DetruireTousCaisses();
    }
}
