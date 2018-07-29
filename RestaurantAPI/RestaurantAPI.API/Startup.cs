using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestaurantAPI.API.Models;
using RestaurantAPI.Data;

namespace RestaurantAPI.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<Library.Repos.AppUserRepo>();
            services.AddScoped<Library.Repos.KeywordRepo>();
            services.AddScoped<Library.Repos.QueryRepo>();
            services.AddScoped<Library.Repos.RestaurantRepo>();
            services.AddDbContext<Data.Project2DBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Project2DB"),
                 b => b.MigrationsAssembly("RestaurantAPI.API")));
            //services.AddDbContext<Project2DBContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("Project2DB"),
            //    b => b.MigrationsAssembly("RestaurantAPI.API")));


            //Setting Up Identity Environment
            services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Project2ApiAuthDB"),
            b => b.MigrationsAssembly("RestaurantAPI.API")));

            // Add-Migration <diff-migration-name> -Context IdentityDbContext
            // Update-Database -Context IdentityDbContext

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "0123456789" +
                "-._@+";
            })
            .AddEntityFrameworkStores<IdentityDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Project2Auth";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = 401;//Unauthorized
                        return Task.FromResult(0);
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
                        ctx.Response.StatusCode = 403;//Forbidden
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddAuthentication();

            services.AddMvc()
                .AddXmlSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Implementing SwashBuckle.AspNetCore
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "My API", Version = "v1" });

            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Part of Identity and Swagger Implementation
            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1//swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
