using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimulateurApps
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lecture de la configuration de l'application
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
        }
    }
}
