using Autofac;
using Autofac.Extensions.DependencyInjection;
using BusEvenement;
using BusEvenement.Abstractions;
using Magasin.API.IntegrationEvents.EventHandling;
using Magasin.API.IntegrationEvents.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQBus;
using RabbitMQBus.Connection;
using System;

namespace Caisse.API
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
            services.AddTransient<CaisseEtatEventHandler>();

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
            busEvenement.Souscrire<CaisseEtatEvent, CaisseEtatEventHandler>();
            busEvenement.ActiverCanalConsommation();
        }
    }
}
