using System;
using System.Collections.Generic;
using System.Text;
using SimulateurApps.Evenements;

namespace SimulateurApps.Services
{
    public interface IApiConnecteur
    {
        System.Threading.Tasks.Task<bool> EnvoyerEvenementAsync(CaisseEtatEvt caisseEtatEvt);

        System.Threading.Tasks.Task<bool> EnvoyerEvenementAsync(CaisseClientEvt caisseClientEvt);
    }
}
