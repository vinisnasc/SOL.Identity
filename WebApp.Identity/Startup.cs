using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApp.Identity
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
            services.AddControllersWithViews();

            // Configuração do BD
            var connectionString = "Integrated Security = SSPI;Persist Security Info=False;" +
                                   "Initial Catalog=IdentityCurso;Data Source=DESKTOP-R9JFMSC\\SQLEXPRESS";
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<MyUserDbContext>(opt => opt.UseSqlServer(connectionString,
                                                     sql => sql.MigrationsAssembly(migrationAssembly)));

            // Inclusão do Identity
            services.AddIdentity<MyUser, IdentityRole>(options => { }).AddEntityFrameworkStores<MyUserDbContext>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");

            // Injeção de dependencia para personalização das Claims
            services.AddScoped<IUserClaimsPrincipalFactory<MyUser>, MyUserClaimsPrincipalFactory>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
                app.UseHsts();
            }

            // Adicionar autenticação
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
