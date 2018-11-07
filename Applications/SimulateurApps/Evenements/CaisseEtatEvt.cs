using System;

namespace SimulateurApps.Evenements
{
    public class CaisseEtatEvt
    {
        public string dateEvenement { get; internal set; }

        public int numero { get; internal set; }

        public string etatCaisseCourant { get; internal set; }

        public CaisseEtatEvt(int _numeroCaisse, string _etat)
        {
            this.dateEvenement = DateTime.Now.Ticks.ToString();
            this.numero = _numeroCaisse;
            this.etatCaisseCourant = _etat;
        }
    }
}
