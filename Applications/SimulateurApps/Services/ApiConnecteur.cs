using System;
using System.Collections.Generic;
using System.Text;
using SimulateurApps.Evenements;

namespace SimulateurApps.Services
{
    public class ApiConnecteur : IApiConnecteur
    {
        public void EnvoyerEvenement(CaisseEtatEvt caisseEtatEvt)
        {
            throw new NotImplementedException();
        }

        public void EnvoyerEvenement(CaisseClientEvt caisseClientEvt)
        {
            throw new NotImplementedException();
        }
    }
}
