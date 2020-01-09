using ADAssignment.Data;
using ADAssignment.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ADAssignment
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
            AzureKeyVaultManager azureKeyVaultManager = new AzureKeyVaultManager();

            var googleAuthSecret = azureKeyVaultManager.GetSecret(
                "https://wessex-key-vault.vault.azure.net/secrets/google-auth-secret/44f451bdf580449e83590706e06485cc");
            var googleClientId = azureKeyVaultManager.GetSecret(
                "https://wessex-key-vault.vault.azure.net/secrets/google-client-id/2fc4e6b1237f410d848e9b9037d7bfdf");
            var dbConnectionString = azureKeyVaultManager.GetSecret(
                "https://wessex-key-vault.vault.azure.net/secrets/db-connection-string/1c8753f9fc8647c3920477fb4dcdf90b");

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(dbConnectionString));

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleAuthSecret;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}