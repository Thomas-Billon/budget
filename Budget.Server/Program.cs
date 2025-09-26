using Budget.Server.Core.Balances;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Helpers;
using Budget.Server.Core.Transactions;
using Budget.Server.Data;
using Budget.Server.Middleware.Error;
using Budget.Server.Middleware.Startup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder()
    .ConfigureCors()
    .ConfigureDbContext()
    .ConfigureExceptionHandlers()
    .ConfigureServices();

var app = builder.Build();

await app.ConfigureApp()
    .ConfigureDbMigration();

app.Run();


public static class ProgramExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            options.SerializerSettings.Converters.Add(new OptionalJsonConverter());
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        return builder;
    }

    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        string[] allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")?.Split(';') ?? [];

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return builder;
    }

    public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        return builder;
    }

    public static WebApplicationBuilder ConfigureExceptionHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<NotImplementedExceptionHandler>();
        builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
        builder.Services.AddExceptionHandler(options =>
        {
            options.ExceptionHandlingPath = "/error"; // required even if unused
        });


        return builder;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDbInitializerService, DbInitializerService>();

        builder.Services.AddScoped<BalanceService>();
        builder.Services.AddScoped<TransactionService>();
        builder.Services.AddScoped<CategoryService>();

        return builder;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.MapStaticAssets();
        app.UseCors();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallbackToFile("/index.html");

        return app;
    }

    public static async Task<WebApplication> ConfigureDbMigration(this WebApplication app)
    {
        using (IServiceScope serviceScope = app.Services.CreateScope())
        {
            IServiceProvider services = serviceScope.ServiceProvider;

            await services.GetRequiredService<IDbInitializerService>().Init(services);
        }

        return app;
    }
}
