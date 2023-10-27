namespace SoftwareFest.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Serilog;
    using SoftwareFest.MailSending;
    using SoftwareFest.Models;
    using SofwareFest.Infrastructure;

    public static class ServiceCollectionExtensions
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Configuration.AddConfiguration(configBuilder.Build()).Build();

        }
        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            var envConnection = builder.Configuration.GetValue<string>("CONNECTION_STRING");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(!string.IsNullOrEmpty(envConnection) ? envConnection : builder.Configuration.GetConnectionString("DefaultConnection")));
        }
        public static void AddMvc(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllers();
        }
        public static void AddCustomIdentity(this WebApplicationBuilder builder)
        {

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
            {
                cfg.Password.RequireUppercase = false;
                cfg.User.RequireUniqueEmail = true;
                cfg.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration config, string appName)
        {
            var seqServerUrl = config.GetValue<string>("Logging:Urls:SeqServerUrl") ?? "http://seq";

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqServerUrl)
                .ReadFrom.Configuration(config)
                .CreateLogger();

            return loggerConfig;
        }
        public static void AddCustomHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!,
                        name: "IdentityDB-check",
                        tags: new string[] { "IdentityDB" });
        }
        public static void AddEmailSending(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<EmailSendingSettings>(builder.Configuration.GetSection("EmailSending"));
            builder.Services.AddTransient<IMailSender, MailSender>();
        }
    }
}
