using Autofac;
using Autofac.Extensions.DependencyInjection;
using BusEvenement;
using BusEvenement.Abstractions;
using Client.API.Bdd.Connexion;
using Client.API.Bdd.Interfaces;
using Client.API.Bdd.Services;
using Client.API.IntegrationEvents.EventHandling;
using Client.API.IntegrationEvents.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQBus;
using RabbitMQBus.Connection;
using System;

namespace Client.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            /*
             * Configuration du Bus.
             */
            services.AddSingleton<IRabbitMQConnexion>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQConnexionDefaut>>();

                var factory = new ConnectionFactory() { HostName = Configuration.GetSection("RabbitMqBus:Connexion").Value };
                factory.UserName = Configuration.GetSection("RabbitMqBus:Utilisateur").Value;
                factory.Password = Configuration.GetSection("RabbitMqBus:MDP").Value;

                var nombreTentative = 5;
                if ((Configuration.GetSection("RabbitMqBus:NombreTentative").Exists()) &&
                    (!string.IsNullOrEmpty(Configuration.GetSection("RabbitMqBus:NombreTentative").Value)))
                    nombreTentative = int.Parse(Configuration.GetSection("RabbitMqBus:NombreTentative").Value);

                return new RabbitMQConnexionDefaut(factory, logger, nombreTentative);
            });
            services.AddSingleton<IBusEvenement, BusEvenementRabbitMQ>(sp =>
            {
                var rabbitMQConnexion = sp.GetRequiredService<IRabbitMQConnexion>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<BusEvenementRabbitMQ>>();
                var busEvenementAboManager = sp.GetRequiredService<IBusEvenementAboManager>();

                var nombreTentative = 5;
                if ((Configuration.GetSection("RabbitMqBus:NombreTentative").Exists()) &&
                    (!string.IsNullOrEmpty(Configuration.GetSection("RabbitMqBus:NombreTentative").Value)))
                    nombreTentative = int.Parse(Configuration.GetSection("RabbitMqBus:NombreTentative").Value);

                return new BusEvenementRabbitMQ(rabbitMQConnexion, logger, iLifetimeScope,
                    busEvenementAboManager, nombreTentative);
            });
            services.AddSingleton<IBusEvenementAboManager, BusEvenementAboManagerDefaut>();
            services.AddTransient<CaisseClientEventHandler>();

            /*
             * Configuration de MongoDB.
             */
            services.Configure<MongoDbConfig>(Options =>
            {
                Options.ChaineConnexion = Configuration.GetSection("MongoDbConfig:ChaineConnexion").Value;
                Options.BaseDeDonnees = Configuration.GetSection("MongoDbConfig:BaseDeDonnees").Value;

                Options.Utilisateur = Configuration.GetSection("MongoDbConfig:Utilisateur").Value;
                Options.MotDePasse = Configuration.GetSection("MongoDbConfig:MDP").Value;
            });
            services.AddSingleton<IMongoDbClient>(sp =>
            {
                ILogger<MongoDbClient> logger = sp.GetRequiredService<ILogger<MongoDbClient>>();
                IOptions<MongoDbConfig> options = sp.GetRequiredService<IOptions<MongoDbConfig>>();
                return new MongoDbClient(options, logger);
            });
            services.AddSingleton<IClientsService>(sp =>
            {
                return new ClientsService(sp.GetRequiredService<IMongoDbClient>());
            });

            /*
             * Configuration de AutoFac.
             * Inversion de contrôle.
             */
            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // Configuration des souscriptions sur le bus.
            var busEvenement = app.ApplicationServices.GetRequiredService<IBusEvenement>();
            busEvenement.Souscrire<CaisseClientEvent, CaisseClientEventHandler>();
            busEvenement.ActiverCanalConsommation();
        }
    }
}
