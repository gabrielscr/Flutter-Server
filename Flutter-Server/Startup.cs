using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flutter.Server.Infra;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Flutter_Server
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFeatureFolders();

            services.AddMediatR();

            services.AddDbContext<AdminContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("ServerConnection"), opt => opt.EnableRetryOnFailure());
            });

            services.AddCors(opts =>
            {
                opts.AddPolicy("Dev", opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseCors("Dev");
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc(opts =>
            {
                opts.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });

            if (env.IsDevelopment())
            {
                app.UseWelcomePage();
            }
        }

    }
}