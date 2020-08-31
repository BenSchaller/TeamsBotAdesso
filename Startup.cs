// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.BotBuilderSamples.Bots;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;

namespace Microsoft.BotBuilderSamples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, TeamsBot>();


            //Create QnAMaker Endpoint as a Singleton
            //Singleton = Von einer Klasse nur ein Objekt
            services.AddSingleton(new QnAMakerEndpoint
            {
                KnowledgeBaseId = Configuration.GetValue<string>($"QnAKnowledgebaseID"),
                EndpointKey = Configuration.GetValue<string>($"QnAAuthKey"),
                Host = Configuration.GetValue<string>($"QnAEndpointHostName")
            });

            var luisApplication = new LuisApplication(
                Configuration[$"LuisAppId"],
                Configuration["LuisAPIKey"],
                (Configuration["LuisAPIHostName"])
            );

            //services.AddSingleton(new SqlConnection(Configuration[$"SQLConnectionString"]));

            // Entity Framework Core Database First

            //Create Luis Endpoint as a Singleton
            services.AddSingleton(new LuisRecognizerOptionsV3(luisApplication)
            { 
                PredictionOptions = new Bot.Builder.AI.LuisV3.LuisPredictionOptions 
                { 
                    IncludeAllIntents = true, IncludeInstanceData = true 
                } 
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
        

    }
}
