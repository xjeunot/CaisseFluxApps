using BusEvenement.Abstractions;
using BusEvenement.Evenement;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusEvenement
{
    public interface IBusEvenementAboManager
    {
        event EventHandler<string> EstEvenementSupprime;

        bool EstVide { get; }

        void Nettoyer();

        void AjouterSouscriptionDynamique<TH>(string nomEvenement) where TH : IDynamiqueEvenementHandler;

        void AjouterSouscription<T, TH>()
            where T : StandardEvenement
            where TH : IStandardEvenementHandler<T>;

        void RetirerSouscription<T, TH>()
            where TH : IStandardEvenementHandler<T>
            where T : StandardEvenement;

        void RetirerSouscriptionDynamique<TH>(string nomEvenement)where TH : IDynamiqueEvenementHandler;

        IEnumerable<SouscriptionInfo> DonneHandlersPourEvenement<T>() where T : StandardEvenement;

        IEnumerable<SouscriptionInfo> DonneHandlersPourEvenement(string nomEvenement);

        bool ExisteSouscriptionPourEvenement<T>() where T : StandardEvenement;

        bool ExisteSouscriptionPourEvenement(string nomEvenement);

        Type DonneTypeEvenement(string nomEvenement);

        string DonneCleEvenement<T>();
    }
}
