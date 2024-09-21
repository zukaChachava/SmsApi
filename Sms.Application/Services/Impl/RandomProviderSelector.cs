using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sms.Application.InfrastructureServices;
using Sms.Application.Options;

namespace Sms.Application.Services.Impl;

public sealed class RandomProviderSelector(
    IServiceScopeFactory serviceScopeFactory,
    IOptionsMonitor<SmsProvidersOptions> smsProviderOptions,
    ILogger<RandomProviderSelector> logger
    ) : IProviderSelector
{
    public ISmsProvider GetSmsProvider()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var providersConfig = smsProviderOptions.CurrentValue.Providers.Select(p => p.Key).ToArray();

        int selection = Random.Shared.Next(0, providersConfig.Length - 1);

        var provider = scope.ServiceProvider.GetKeyedService<ISmsProvider>(providersConfig[selection]);

        if(provider is null)
            throw new InvalidOperationException($"No such provider registered: {providersConfig[selection]}");

        logger.LogInformation($"RandomProviderSelector: Provider: {provider}");
        return provider;
    }
}