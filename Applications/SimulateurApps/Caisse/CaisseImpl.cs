using SimulateurApps.Evenements;
using SimulateurApps.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SimulateurApps.Caisse
{
    public class CaisseImpl
    {
        private int NumeroCaisse;
        private IApiConnecteur apiConnecteur;
        private double TempsAttenteClient;
        private double TempsTraitementClient;
        protected int NombreClientMaximum;

        private EtatCaisse EtatCaisse;

        public CaisseImpl(int _numeroCaisse, IApiConnecteur _apiConnecteur,
            double _tempsAttenteClient, double _tempsTraitementClient,
            int _nombreClientMaximum)
        {
            this.NumeroCaisse = _numeroCaisse;
            this.apiConnecteur = _apiConnecteur;
            this.TempsAttenteClient = _tempsAttenteClient;
            this.TempsTraitementClient = _tempsTraitementClient;
            this.NombreClientMaximum = _nombreClientMaximum;
        }

        #region Traitement

        public void Traitement()
        {
            // On ouvre la caisse.
            this.OuvertureCaisse();

            // Boucle de traitement.
            while (this.EtatCaisse != EtatCaisse.Ferme)
            {
                // Prochain client.
                string strClientSuivant = this.DonneClientSuivant();

                // Cas de client dans l'immédiat.
                if (strClientSuivant == string.Empty)
                {
                    // Caisse ouverte : on attend.
                    if (this.EtatCaisse == EtatCaisse.Ouverte)
                        Thread.Sleep(TimeSpan.FromSeconds(this.TempsAttenteClient));

                    // Caisse en cours de fermeture : on ferme.
                    if (this.EtatCaisse == EtatCaisse.DernierClient)
                        this.FermeCaisse();
                }
                // Traitement du client courant.
                else
                {
                    this.EvenementClient(strClientSuivant, EvenementClientType.DebutClient);
                    Thread.Sleep(TimeSpan.FromSeconds(this.TempsTraitementClient));
                    this.EvenementClient(strClientSuivant, EvenementClientType.FinClient);
                }
            }
        }

        private string DonneClientSuivant()
        {
            return $"Client_{DateTime.Now.Second}{DateTime.Now.Millisecond}";
        }

        #endregion

        #region Changement Etat Caisse

        private void OuvertureCaisse()
        {
            // Ouverture.
            if (this.EtatCaisse == EtatCaisse.Ferme)
            {
                this.EtatCaisse = EtatCaisse.Ouverte;
                EvenementEtatCaisse();
            }
        }

        public void DernierClient()
        {
            // Dernier Client.
            if (this.EtatCaisse == EtatCaisse.Ouverte)
            {
                this.EtatCaisse = EtatCaisse.DernierClient;
                EvenementEtatCaisse();
            }
        }

        public void FermeCaisse()
        {
            // Fermeture.
            if (this.EtatCaisse == EtatCaisse.DernierClient)
            {
                this.EtatCaisse = EtatCaisse.Ferme;
                EvenementEtatCaisse();
            }
        }

        #endregion

        #region Evenements

        private void EvenementEtatCaisse()
        {
            this.apiConnecteur.EnvoyerEvenement(new CaisseEtatEvt(this.NumeroCaisse, this.EtatCaisse.ToString()));
        }

        private void EvenementClient(string _numeroClient, EvenementClientType _evenementClientType)
        {
            this.apiConnecteur.EnvoyerEvenement(new CaisseClientEvt(this.NumeroCaisse, _numeroClient, _evenementClientType.ToString()));
        }

        #endregion
    }

    internal enum EvenementClientType
    {
        DebutClient,
        FinClient
    }
}
