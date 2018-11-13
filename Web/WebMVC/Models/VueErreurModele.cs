using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebMVC.Models
{
    public class VueErreurModele
    {
        public string Requete { get; internal set; }

        public VueErreurModeleType TypeErreur { get; internal set; }

        public string Message { get; internal set; }

        public string ExceptionMessage { get; internal set; }

        // Construction par l'exception déclenchée par HttpClient.
        public VueErreurModele(HttpRequest requete, HttpRequestException exception)
        {
            this.Requete =
                requete.Protocol + " " +
                requete.Method + " " +
                requete.Host + requete.Path;
            this.TypeErreur = VueErreurModeleType.Api;
            this.Message = "Problème d'appel à une API externe";
            this.ExceptionMessage = exception.Message;
        }

        // Erreur 404 API Externe.
        public VueErreurModele(HttpRequest requete, AggregateException exception)
        {
            this.Requete =
                requete.Protocol + " " +
                requete.Method + " " +
                requete.Host + requete.Path;
            this.TypeErreur = VueErreurModeleType.Api;
            this.Message = "Problème d'appel à une API externe";
            this.ExceptionMessage = exception.Message;
        }

        // Construction par l'absence d'un ou plusieurs argument(s) dans l'URL.
        public VueErreurModele(HttpRequest requete, IList<string> listeArgumentsManquant)
        {
            this.Requete =
                requete.Protocol + " " +
                requete.Method + " " +
                requete.Host + requete.Path;
            this.TypeErreur = VueErreurModeleType.UrlArgument;
            this.Message = "Il manque un ou plusieurs argument(s) dans l'URL";
            this.ExceptionMessage = this.DonneExceptionMessage(listeArgumentsManquant);
        }

        private string DonneExceptionMessage(IList<string> listeArgumentsManquant)
        {
            string strRetour = "Liste des arguments manquants : ";
            for(int i = 0; i < listeArgumentsManquant.Count; i++)
            {
                if (i != 0) strRetour += " , ";
                strRetour += listeArgumentsManquant[i];
            }
            return strRetour;
        }

        public enum VueErreurModeleType
        {
            Api,
            UrlArgument
        }
    }
}
