using System;

namespace SimulateurApps.Evenements
{
    public class CaisseEtatEvt
    {
        public DateTime dateEvenement { get; internal set; }

        public int Numero { get; internal set; }

        public string EtatCaisseCourant { get; internal set; }

        public CaisseEtatEvt(int _numeroCaisse, string _etat)
        {
            this.dateEvenement = DateTime.Now;
            this.Numero = _numeroCaisse;
            this.EtatCaisseCourant = _etat;
        }
    }
}
