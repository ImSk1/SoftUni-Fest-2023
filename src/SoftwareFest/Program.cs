using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SoftwareFest.Infrastructure.Extensions;
using SoftwareFest.MailSending;
using SoftwareFest.Services;
using SoftwareFest.Services.Contracts;
using Stripe;
using System.Diagnostics.CodeAnalysis;
using ProductService = SoftwareFest.Services.ProductService;
using ServiceCollectionExtensions = SoftwareFest.Infrastructure.Extensions.ServiceCollectionExtensions;

var appName = "SoftUni-Fest";
var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.AddConfiguration();
builder.AddMvc();
builder.AddDatabase();
builder.AddCustomHealthChecks();
builder.AddCustomIdentity();
builder.AddEmailSending();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IRetailerService, RetailerService>();
builder.Services.AddTransient<IMailSender, MailSender>();
builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog(ServiceCollectionExtensions.CreateSerilogLogger(builder.Configuration, appName));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}"
);

app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
});
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});
try
{
    app.Logger.LogInformation("Migrating database for {ApplicationName}...", appName);

    using var scope = app.Services.CreateScope();

    //await DatabaseMiddleware.MigrateDatabase(scope, app.Configuration, app.Logger);

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", appName);

    throw;
}
finally
{
    Serilog.Log.CloseAndFlush();
}

[ExcludeFromCodeCoverage]
public partial class Program
{
    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}
