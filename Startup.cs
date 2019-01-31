using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using angular_signalr.Services;
using angular_signalr.Hubs;
using angular_signalr.Models;

namespace angular_signalr
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();
            services.AddSingleton<ITimedBackgroundService, TimedBackgroundService>();
            services.AddSingleton<IHttpGameChannelService, HttpGameChannelService>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/ClientApp";
            });

            // configure named HttpClients
            services.AddHttpClient("Far_Cry_5", c =>
            {
                c.BaseAddress = new Uri(Configuration["Twitch_API:Streams:Far_Cry_5:LookupById"]);               
                c.DefaultRequestHeaders.Add("Client-ID", Configuration["Twitch_API:Client-ID"]);
                
            });

            services.AddHttpClient("Assassins_Creed_Odyssey", c =>
            {
                c.BaseAddress = new Uri(Configuration["Twitch_API:Streams:Assassins_Creed_Odyssey:LookupById"]);               
                c.DefaultRequestHeaders.Add("Client-ID", Configuration["Twitch_API:Client-ID"]);
                
            });

            services.AddHttpClient("Tom_Clancys_Rainbow_Six_Siege", c =>
            {
                c.BaseAddress = new Uri(Configuration["Twitch_API:Streams:Tom_Clancys_Rainbow_Six_Siege:LookupById"]);               
                c.DefaultRequestHeaders.Add("Client-ID", Configuration["Twitch_API:Client-ID"]);
                
            });

            services.AddHttpClient("All_Games", c =>
            {
                c.BaseAddress = new Uri(Configuration["Twitch_API:Streams:All_Games:LookupById"]);               
                c.DefaultRequestHeaders.Add("Client-ID", Configuration["Twitch_API:Client-ID"]);
                
            });
            services.AddOptions();
    
            // The game statistics models have some preconfigured settings that we inject
            services.Configure<List<GameStatsModel>>(Configuration.GetSection("Twitch_API:Games"));
             

        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaul t",
                    template: "{controller}/{action=Index}/{id?}");
            });

           app.UseSignalR(options => 
            {
                options.MapHub<ChatHub>("/hub");
            });
 
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            
            ITimedBackgroundService timedBackgroundService = 
                app.ApplicationServices.GetRequiredService<ITimedBackgroundService>();
            
            timedBackgroundService.StartAsync();
        }
    }
}
