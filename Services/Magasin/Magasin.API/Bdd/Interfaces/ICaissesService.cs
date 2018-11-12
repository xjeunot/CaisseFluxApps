using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magasin.API.Bdd.Interfaces
{
    public interface ICaissesService
    {
        Task<IEnumerable<Models.CaisseItem>> DonneCaisses();

        Task<Models.CaisseItem> DonneCaisse(string id);

        Task<Models.CaisseItem> RechercherCaisseUniqueAvecNumero(int numero);

        void AjouterCaisse(Models.CaisseItem model);

        Task<bool> MajCaisse(Models.CaisseItem model);

        Task<DeleteResult> DetruireCaisse(string id);

        Task<DeleteResult> DetruireToutesCaisses();
    }
}
