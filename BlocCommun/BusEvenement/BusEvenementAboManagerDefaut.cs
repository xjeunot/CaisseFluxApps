using BusEvenement.Abstractions;
using BusEvenement.Evenement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusEvenement
{
    public class BusEvenementAboManagerDefaut : IBusEvenementAboManager
    {
        private readonly Dictionary<string, List<SouscriptionInfo>> _handlers;
        private readonly List<Type> _typeEvenement;

        public event EventHandler<string> EstEvenementSupprime;

        public BusEvenementAboManagerDefaut()
        {
            _handlers = new Dictionary<string, List<SouscriptionInfo>>();
            _typeEvenement = new List<Type>();
        }

        public bool EstVide => !_handlers.Keys.Any();

        public void Nettoyer() => _handlers.Clear();

        public void AjouterSouscriptionDynamique<TH>(string nomEvenement)
            where TH : IDynamiqueEvenementHandler
        {
            FaireAjouterSouscription(typeof(TH), nomEvenement, estDynamique: true);
        }

        public void AjouterSouscription<T, TH>()
            where T : StandardEvenement
            where TH : IStandardEvenementHandler<T>
        {
            var nomEvenement = DonneCleEvenement<T>();
            FaireAjouterSouscription(typeof(TH), nomEvenement, estDynamique: false);
            _typeEvenement.Add(typeof(T));
        }

        public void RetirerSouscription<T, TH>()
            where TH : IStandardEvenementHandler<T>
            where T : StandardEvenement
        {
            var handlerARetirer = TrouveSouscriptionARetirer<T, TH>();
            var nomEvenement = DonneCleEvenement<T>();
            FaireRetirerHandler(nomEvenement, handlerARetirer);
        }

        public void RetirerSouscriptionDynamique<TH>(string nomEvenement)
            where TH : IDynamiqueEvenementHandler
        {
            var handlerARetirer = TrouveSouscriptionDynamiqueARetirer<TH>(nomEvenement);
            FaireRetirerHandler(nomEvenement, handlerARetirer);
        }

        public IEnumerable<SouscriptionInfo> DonneHandlersPourEvenement<T>() where T : StandardEvenement
        {
            var cle = DonneCleEvenement<T>();
            return DonneHandlersPourEvenement(cle);
        }

        public IEnumerable<SouscriptionInfo> DonneHandlersPourEvenement(string nomEvenement) => _handlers[nomEvenement];

        public bool ExisteSouscriptionPourEvenement<T>() where T : StandardEvenement
        {
            var cle = DonneCleEvenement<T>();
            return ExisteSouscriptionPourEvenement(cle);
        }

        public bool ExisteSouscriptionPourEvenement(string nomEvenement) => _handlers.ContainsKey(nomEvenement);

        public Type DonneTypeEvenement(string eventName) => _typeEvenement.SingleOrDefault(t => t.Name == eventName);

        public string DonneCleEvenement<T>()
        {
            return typeof(T).Name;
        }

        private void FaireRetirerHandler(string nomEvenement, SouscriptionInfo subsARetirer)
        {
            if (subsARetirer != null)
            {
                _handlers[nomEvenement].Remove(subsARetirer);
                if (!_handlers[nomEvenement].Any())
                {
                    _handlers.Remove(nomEvenement);
                    var typeEvenement = _typeEvenement.SingleOrDefault(e => e.Name == nomEvenement);
                    if (typeEvenement != null)
                    {
                        _typeEvenement.Remove(typeEvenement);
                    }
                    TerminerRetirerEvenement(nomEvenement);
                }
            }
        }

        private void TerminerRetirerEvenement(string nomEvenement)
        {
            var handler = EstEvenementSupprime;
            if (handler != null)
            {
                EstEvenementSupprime(this, nomEvenement);
            }
        }

        private void FaireAjouterSouscription(Type typeHandler, string nomEvenement, bool estDynamique)
        {
            if (!ExisteSouscriptionPourEvenement(nomEvenement))
            {
                _handlers.Add(nomEvenement, new List<SouscriptionInfo>());
            }

            if (_handlers[nomEvenement].Any(s => s.TypeHandler == typeHandler))
            {
                throw new ArgumentException(
                    $"Le type de Handler {typeHandler.Name} est déjà enregistré pour '{nomEvenement}'", nameof(typeHandler));
            }

            if (estDynamique)
            {
                _handlers[nomEvenement].Add(SouscriptionInfo.Dynamique(typeHandler));
            }
            else
            {
                _handlers[nomEvenement].Add(SouscriptionInfo.Typed(typeHandler));
            }
        }

        private SouscriptionInfo TrouveSouscriptionDynamiqueARetirer<TH>(string nomEvenement)
            where TH : IDynamiqueEvenementHandler
        {
            return FaireTrouveSouscriptionARetirer(nomEvenement, typeof(TH));
        }

        private SouscriptionInfo TrouveSouscriptionARetirer<T, TH>()
             where T : StandardEvenement
             where TH : IStandardEvenementHandler<T>
        {
            var nomEvenement = DonneCleEvenement<T>();
            return FaireTrouveSouscriptionARetirer(nomEvenement, typeof(TH));
        }

        private SouscriptionInfo FaireTrouveSouscriptionARetirer(string nomEvenement, Type typeHandler)
        {
            if (!ExisteSouscriptionPourEvenement(nomEvenement))
            {
                return null;
            }
            return _handlers[nomEvenement].SingleOrDefault(s => s.TypeHandler == typeHandler);
        }
    }
}
