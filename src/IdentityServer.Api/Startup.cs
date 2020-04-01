using System.Reflection;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Services;
using IdentityServer.Core.CommandHandlers;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        
        public IConfiguration Configuration { get; set; }

        public IHostEnvironment Environment;
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddControllers();
            
            services.AddCors(
                options =>
                {
                    options.AddPolicy(
                        "CorsPolicy",
                        builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });


            services.AddControllersWithViews();
            services.AddRazorPages();
            
            services.ConfigureIdentityServer(Configuration, Environment);

            // services.AddTransient<ITokenCreationService, JwtTokenCreationService>();
            

            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(UserCommandsHandler).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
   
            app.UseRouting();
            
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("CorsPolicy");
            app.UseIdentityServer();

            
            
            
         
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            
        }
    }
}
