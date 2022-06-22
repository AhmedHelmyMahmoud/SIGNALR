using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace NgChatSignalR
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()

                    .WithOrigins("https://localhost:44335")
                    .WithOrigins("http://localhost:4200")
                    .WithOrigins("http://localhost:51337")
                    .WithOrigins("https://ng-chat.azurewebsites.net")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            }));

            services
                .AddSignalR();
              //  .AddAzureSignalR();
                //.AddJsonProtocol(options =>
                //{
                //    options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //    //options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //});

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseFileServer();

          //  app.UseHttpsRedirection();
            app.UseStaticFiles();
        //    app.UseCookiePolicy();

            app.UseCors("CorsPolicy");

            //app.UseAzureSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/chat");
            //    routes.MapHub<GroupChatHub>("/groupchat");
            //});
            app.UseEndpoints(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
                routes.MapHub<GroupChatHub>("/groupchat");

                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                routes.MapControllerRoute(
             name: "actionOnly",
             pattern: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRazorPages();

            });
            app.UseEndpoints(endpoints =>
            {

            });


            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "actionOnly",
            //        template: "{action}",
            //        defaults: new { controller = "Home", action = "Index" }
            //    );

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
