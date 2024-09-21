using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Sms.Application.Options;
using Sms.Application.Pipelines;
using Sms.Application.Services;
using Sms.Application.Services.Impl;

namespace Sms.Application.Extensions;

public static class RegisterApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddOptions<ProviderSelectorOptions>()
            .BindConfiguration(nameof(ProviderSelectorOptions))
            .ValidateOnStart();

        services.AddOptions<SmsProvidersOptions>()
            .BindConfiguration(nameof(SmsProvidersOptions))
            .ValidateOnStart();

        services.AddValidatorsFromAssembly(typeof(RegisterApplication).Assembly);

        services.AddSingleton<ISmsProviderFactory, SmsProviderFactory>();

        services.AddKeyedSingleton<IProviderSelector, RandomProviderSelector>("RandomProviderSelector");
        services.AddKeyedSingleton<IProviderSelector, PercentProviderSelector>("PercentProviderSelector");

        services.AddMediatR(config =>
        {
            config.AddOpenBehavior(typeof(ValidatorPipeline<,>));
            config.RegisterServicesFromAssembly(typeof(IApplicationAssemblyReference).Assembly);
        });

        return services;
    }
}