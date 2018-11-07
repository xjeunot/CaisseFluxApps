using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IClientService
    {
        Task<IEnumerable<string>> DonneClients();

        Task<ClientDTO> DonneClient(string id);
    }
}
