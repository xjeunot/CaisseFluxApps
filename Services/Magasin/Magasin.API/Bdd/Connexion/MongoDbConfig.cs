using System.Security;

namespace Client.API.Bdd.Connexion
{
    public class MongoDbConfig
    {
        public string ChaineConnexion { get; set; }

        public string BaseDeDonnees { get; set; }

        public string Utilisateur { get; set; }

        public string MotDePasse { get; set; }
    }
}
