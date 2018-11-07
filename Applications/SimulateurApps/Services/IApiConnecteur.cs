using System;
using System.Collections.Generic;
using System.Text;
using SimulateurApps.Evenements;

namespace SimulateurApps.Services
{
    public interface IApiConnecteur
    {
        void EnvoyerEvenement(CaisseEtatEvt caisseEtatEvt);
        void EnvoyerEvenement(CaisseClientEvt caisseClientEvt);
    }
}
