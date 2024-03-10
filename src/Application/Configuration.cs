using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Application.Validations;
using Application.Logging;
using Application.Engines.Cryptography;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Application.Engines.DataControl;

namespace Application
{
    [ExcludeFromCodeCoverage]
    public static class Configuration
    {
        public static void AddApplicationConfiguration(this IServiceCollection services, PasswordValidation passwordValidatorConfiguration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.RegisterServicesFromAssembly(typeof(Configuration).Assembly);
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(Configuration).Assembly);
            services.AddSingleton<IPasswordValidation>(passwordValidatorConfiguration);
            services.AddTransient<ITextCryptography, TextCryptography>();
            services.AddTransient<IUserDataControl, UserDataControl>();
        }
    }
}
