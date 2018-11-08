using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IClientService
    {

        IEnumerable<string> DonneClients();

        ClientDTO DonneClient(string id);
    }
}
