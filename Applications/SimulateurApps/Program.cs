using Microsoft.Extensions.Configuration;
using SimulateurApps.Caisse;
using SimulateurApps.Services;
using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace SimulateurApps
{
    class Program
    {
        private static int nbCaisses = 0;
        private static double tempsAttenteClient = 0;
        private static double tempsTraitementClient = 0;
        private static int tempsOuverture = 0;

        static void Main(string[] args)
        {
            // Lecture de la configuration de l'application
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            // Création de la collection de services.
            ServiceCollection services = new ServiceCollection();

            // Ajout client Http : ApiSimulationFluxV1
            services.AddHttpClient("ApiSimulationFluxV1", client =>
            {
                client.BaseAddress = new Uri(configuration["ApiSimulationFluxV1"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddTransientHttpErrorPolicy(builders => builders.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }));

            // Ajout des API Services.
            services.AddTransient<IApiConnecteur, ApiConnecteur>();

            // Build des services.
            ServiceProvider clsServiceProvider = services.BuildServiceProvider();

            // Lecture des paramètres.
            nbCaisses = int.Parse(configuration["NombreDeCaisse"]);
            tempsAttenteClient = double.Parse(configuration["TempsAttenteClient"]);
            tempsTraitementClient = double.Parse(configuration["TempsTraitementClient"]);
            tempsOuverture = int.Parse(configuration["TempsOuverture"]);

            // Création des caisses.
            for (int i = 0; i < nbCaisses; i++)
            {
                CaisseImpl clsCaisseCourant = new CaisseImpl(i + 1,
                    clsServiceProvider.GetRequiredService<IApiConnecteur>(),
                    tempsAttenteClient, tempsTraitementClient, tempsOuverture);

                Thread clsThread;
                clsThread = new Thread(new ThreadStart(clsCaisseCourant.Traitement));
                clsThread.Start();

                Thread.Sleep(TimeSpan.FromSeconds(tempsTraitementClient/nbCaisses));
            }
        }
    }
}
