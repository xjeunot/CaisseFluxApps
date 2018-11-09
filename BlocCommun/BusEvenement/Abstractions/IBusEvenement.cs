using BusEvenement.Evenement;

namespace BusEvenement.Abstractions
{
    public interface IBusEvenement
    {
        void Publier(StandardEvenement @evenement);

        void ActiverCanalConsommation();

        void Souscrire<T, TH>()
            where T : StandardEvenement
            where TH : IStandardEvenementHandler<T>;

        void SouscrireDynamiquement<TH>(string nomEvenement)
            where TH : IDynamiqueEvenementHandler;

        void ResilierDynamiquement<TH>(string nomEvenement)
            where TH : IDynamiqueEvenementHandler;

        void Resilier<T, TH>()
            where TH : IStandardEvenementHandler<T>
            where T : StandardEvenement;
    }
}
