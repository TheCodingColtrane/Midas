using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Midas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Midas.Models;
using Microsoft.AspNetCore.Identity;

namespace Midas
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
            SqlConnectionStringBuilder sqlCrendencials = new();
            sqlCrendencials.DataSource = Configuration["SRV"];
            sqlCrendencials.InitialCatalog = Configuration["IC"];
            sqlCrendencials.UserID = Configuration["US"];
            sqlCrendencials.Password = Configuration["PWD"];
            //services.AddTransient<IDbConnection>(con => new SqlConnection(sqlCrendencials.ConnectionString));
            services.AddControllersWithViews();
            services.AddDbContext<MidasContext>(c => c.UseSqlServer(sqlCrendencials.ConnectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MidasContext>()
                .AddDefaultTokenProviders();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Midas", Version = "v1" });
            });
            services.AddSession(s =>
            {
                s.IdleTimeout = TimeSpan.FromMinutes(30);
                s.IOTimeout = TimeSpan.FromSeconds(30);
                s.Cookie.Name = "midassess";
            });

            
            services.AddCors(options => options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();}));
            
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Midas v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute("APIs", "api/{controller=Home}/{action=Index}/{id?}", new { controller = "Home", action = "Index", id = 0 }
                //new string[] { "Midas.APIs" });
            });
        }
    }
}
