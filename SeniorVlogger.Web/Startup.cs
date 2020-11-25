using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeniorVlogger.Common;
using SeniorVlogger.Common.Email;
using SeniorVlogger.Common.Email.IEmail;
using SeniorVlogger.DataAccess.Data;
using SeniorVlogger.DataAccess.Repository;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Web.Services;
using VueCliMiddleware;

namespace SeniorVlogger.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SqliteConnection"),
                sqliteOptions => sqliteOptions.MigrationsAssembly("SeniorVlogger.SqliteMigrations"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IEmailService, EmailService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            services.AddSingleton<UploadsService>();
            services.AddSingleton<JwtService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
            services.AddTokenAuthentication(Configuration);

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp/dist");
            app.UseSpaStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions {SourcePath = "ClientApp"},
                    npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                    regex: "Compiled successfully",
                    forceKill: true,
                    wsl: false);
            });



        }
    }
}
