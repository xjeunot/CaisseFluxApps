using System;
using System.Collections.Generic;
using System.Text;
using SimulateurApps.Evenements;

namespace SimulateurApps.Services
{
    public interface IApiConnecteur
    {
        bool EnvoyerEvenement(CaisseEtatEvt caisseEtatEvt);

        bool EnvoyerEvenement(CaisseClientEvt caisseClientEvt);
    }
}
