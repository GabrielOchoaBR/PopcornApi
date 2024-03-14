using System.Text;
using Application.Engines.DataControl;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PopcornApi.Controllers.V1;
using PopcornApi.Middlewares;
using PopcornApi.Model.Settings;
using PopcornApi.Security.TokenServices;
using PopcornApi.Swagger;

namespace PopcornApi.Configuration
{
    public static class PopcornApiConfiguration
    {
        public static void AddPopcornApiConfiguration(this IServiceCollection services, AuthenticationSettings authenticationSettings)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PopcornApiConfiguration).Assembly));
            services.AddTransient<ExceptionHandlerMiddleware>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,

                        ClockSkew = TimeSpan.Zero,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Key))
                    };
                });

            services.AddSwaggerGen(x =>
            {
                x.SchemaFilter<LoginSampleFilter>();
                x.SchemaFilter<GetAllSampleFilter>();

                x.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, $"{typeof(PopcornApiConfiguration).Assembly.GetName().Name}.xml"));

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddAuthorizationBuilder()
                .AddPolicy("Read", policy => policy.RequireRole("Read"))
                .AddPolicy("Write", policy => policy.RequireRole("Write"));

            services.AddRateLimiter(rateLimiter =>
            {
                rateLimiter.AddFixedWindowLimiter("LimitedAttempted", options =>
                {
                    options.PermitLimit = 6;
                    options.QueueLimit = 3;
                    options.Window = TimeSpan.FromSeconds(10);
                });
                rateLimiter.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<QueryController>()
                .AddMongoDbProjections()
                .AddMongoDbFiltering()
                .AddMongoDbSorting();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAppSettings>(x => AppSettingsConfiguration.GetSettings());
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthorizationService, TokenService>();
        }
    }
}
