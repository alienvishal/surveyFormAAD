using eXtolloURLWhitelist.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

namespace eXtolloURLWhitelist
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => 
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages();
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApp(config.GetSection("AzureAD"));
            services.AddDbContextPool<SurveyDBContext>(option => option.UseSqlServer(config.GetConnectionString("Survey")));
            services.AddScoped<ISurveyRepository, SurveyRepository>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin-only", p =>
                {
                    p.RequireClaim("groups", "d83ce8ca-4664-4c5c-a687-43bfb48ff975");
                });

                options.AddPolicy("users-only", p => {
                    p.RequireClaim("groups", "6fc0baee-2dc5-4aa3-9cda-feb0e24afe80");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=CheckSignIn}/{Id?}");
                
                endpoints.MapRazorPages();
            });
        }
    }
}
