using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequiredLength = 3;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IEmailService, EmailService>();
            services.Configure<EmailSettings>(Configuration.GetSection("MailGun"));
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            services.AddSingleton<UploadsService>();
            services.AddSingleton<JwtService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
            services.AddTokenAuthentication(Configuration);

            var env = (IHostEnvironment)services.First(d =>
               d.ServiceType == typeof(IHostEnvironment))?.ImplementationInstance;

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = env.IsDevelopment() ? "ClientApp" : "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            if (string.IsNullOrWhiteSpace(env.WebRootPath))
            {
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(),
                    (env.IsDevelopment()) ? "ClientApp" : "ClientApp/dist");
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseVueCli("serve");
                }
                else
                {
                    spa.Options.SourcePath = "ClientApp/dist";
                }
            });
        }
    }
}
