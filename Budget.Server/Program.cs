using Budget.Server.Core.Auth;
using Budget.Server.Core.Auth.Validation;
using Budget.Server.Core.Balances;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Email.Senders;
using Budget.Server.Core.Errors;
using Budget.Server.Core.Shared;
using Budget.Server.Core.Transactions;
using Budget.Server.Data;
using Budget.Server.Data.Users;
using Budget.Server.Middleware.Background;
using Budget.Server.Middleware.Configuration;
using Budget.Server.Middleware.Conventions;
using Budget.Server.Middleware.Converters;
using Budget.Server.Middleware.Exceptions;
using Budget.Server.Middleware.Startup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Scalar.AspNetCore;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;


var builder = WebApplication
    .CreateBuilder(args)
    .Configure();

var app = await builder
    .Build()
    .Setup()
    .InitDatabase();

app.Run();


public static class ProgramExtensions
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        return builder.ConfigureMiddleware()
            .ConfigureApi()
            .ConfigureForwardedHeaders()
            .ConfigureAuth()
            .ConfigureApp()
            .ConfigureSmtp()
            .ConfigureRateLimiting()
            .ConfigureHsts()
            .ConfigureDbContext()
            .ConfigureExceptionHandlers()
            .ConfigureServices();
    }

    public static WebApplicationBuilder ConfigureMiddleware(this WebApplicationBuilder builder)
    {
        TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));

        return builder;
    }

    public static WebApplicationBuilder ConfigureApi(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers(options =>
            {
                options.Conventions.Add(new ApiRoutePrefixConvention());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.Converters.Add(new OptionalJsonConverter());
            });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key.ToCamelCase(),
                        e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                    );

                return new UnprocessableEntityObjectResult(new ValidationProblemDetails(errors));
            };
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureForwardedHeaders(this WebApplicationBuilder builder)
    {
        var trustedProxies = builder.Configuration.GetValue<string[]>("ReverseProxy:TrustedProxies") ?? [];

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            foreach (var proxy in trustedProxies)
            {
                options.KnownProxies.Add(IPAddress.Parse(proxy));
            }
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
    {
        var authConfiguration = builder.Configuration.GetRequiredSection<AuthConfiguration>(AuthConfiguration.CONFIG_KEY);

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser, ResetPasswordTokenProviderOptions>>(ResetPasswordTokenProviderOptions.PROVIDER_NAME)
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser, EmailConfirmationTokenProviderOptions>>(EmailConfirmationTokenProviderOptions.PROVIDER_NAME);

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = PasswordRules.MinLength;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
            options.Tokens.PasswordResetTokenProvider = ResetPasswordTokenProviderOptions.PROVIDER_NAME;
            options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProviderOptions.PROVIDER_NAME;
        });

        builder.Services.Configure<ResetPasswordTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromSeconds(authConfiguration.ResetPassword.ExpirationInSeconds);
        });

        builder.Services.Configure<EmailConfirmationTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromSeconds(authConfiguration.EmailConfirmation.ExpirationInSeconds);
        });

        builder.Services.AddSingleton(authConfiguration);
        builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // INFO: MapInboundClaims is set to false so we can use claim names instead of .NET-style URIs
            options.MapInboundClaims = false;
            options.TokenValidationParameters.ValidateIssuer = true;
            options.TokenValidationParameters.ValidIssuer = authConfiguration.AccessToken.Issuer;
            options.TokenValidationParameters.ValidateAudience = true;
            options.TokenValidationParameters.ValidAudience = authConfiguration.AccessToken.Audience;
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.AccessToken.SecretKey));
        });

        builder.Services.AddAuthorization();

        return builder;
    }

    public static WebApplicationBuilder ConfigureApp(this WebApplicationBuilder builder)
    {
        var appUrl = builder.Configuration.GetRequiredValue<string>("AppUrl");

        builder.Services.AddSingleton(new AppConfiguration { Url = appUrl });

        return builder;
    }

    public static WebApplicationBuilder ConfigureSmtp(this WebApplicationBuilder builder)
    {
        var smtpConfiguration = builder.Configuration.GetRequiredSection<SmtpConfiguration>(SmtpConfiguration.CONFIG_KEY);

        builder.Services.AddSingleton(smtpConfiguration);
        builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();

        return builder;
    }

    public static WebApplicationBuilder ConfigureRateLimiting(this WebApplicationBuilder builder)
    {
        var rateLimiterConfiguration = builder.Configuration.GetRequiredSection<RateLimiterConfiguration>(RateLimiterConfiguration.CONFIG_KEY);

        builder.Services.AddRateLimiter(options =>
        {
            options.OnRejected = (context, cancellationToken) =>
            {
                TimeSpan? retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var metadata) ? metadata : null;
                throw new TooManyRequestsException(retryAfter);
            };

            options.AddPolicy(RateLimiterConfiguration.RegisterPolicy, rateLimiterConfiguration.Register.GetPartition);
            options.AddPolicy(RateLimiterConfiguration.LoginPolicy, rateLimiterConfiguration.Login.GetPartition);
            options.AddPolicy(RateLimiterConfiguration.ForgotPasswordPolicy, rateLimiterConfiguration.ForgotPassword.GetPartition);
            options.AddPolicy(RateLimiterConfiguration.ResetPasswordPolicy, rateLimiterConfiguration.ResetPassword.GetPartition);
            options.AddPolicy(RateLimiterConfiguration.ConfirmEmailPolicy, rateLimiterConfiguration.ConfirmEmail.GetPartition);
            options.AddPolicy(RateLimiterConfiguration.ResendEmailConfirmationPolicy, rateLimiterConfiguration.ResendEmailConfirmation.GetPartition);
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureHsts(this WebApplicationBuilder builder)
    {
        builder.Services.AddHsts(options =>
        {
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(365);
        });

        return builder;
    }

    public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()
            )
        );

        return builder;
    }

    public static WebApplicationBuilder ConfigureExceptionHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<NotImplementedExceptionHandler>();
        builder.Services.AddExceptionHandler<UnauthorizedExceptionHandler>();
        builder.Services.AddExceptionHandler<TooManyRequestsExceptionHandler>();
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
        builder.Services.AddHostedService<RefreshTokenCleanupService>();

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<BalanceService>();
        builder.Services.AddScoped<TransactionService>();
        builder.Services.AddScoped<CategoryService>();

        return builder;
    }

    public static WebApplication Setup(this WebApplication app)
    {
        app.UseForwardedHeaders();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseExceptionHandler();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.MapStaticAssets();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        app.MapControllers();
        app.MapFallbackToFile("/index.html");

        return app;
    }

    public static async Task<WebApplication> InitDatabase(this WebApplication app)
    {
        using (IServiceScope serviceScope = app.Services.CreateScope())
        {
            IServiceProvider services = serviceScope.ServiceProvider;

            await services.GetRequiredService<IDbInitializerService>().Init(services);
        }

        return app;
    }
}
