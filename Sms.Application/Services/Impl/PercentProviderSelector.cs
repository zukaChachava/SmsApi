using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sms.Application.InfrastructureServices;
using Sms.Application.Options;

namespace Sms.Application.Services.Impl;

public sealed class PercentProviderSelector(
    IServiceScopeFactory serviceScopeFactory,
    IOptionsMonitor<SmsProvidersOptions> smsProviderOptions,
    ILogger<PercentProviderSelector> logger
    )
    : IProviderSelector
{
    public ISmsProvider GetSmsProvider()
    {
        var providersConfiguration = smsProviderOptions.CurrentValue.Providers;

        if(providersConfiguration.Sum(p => p.Value) != 100)
            throw new InvalidOperationException("Providers percentage should be 100%");

        using var scope = serviceScopeFactory.CreateScope();

        int selection = Random.Shared.Next(1, 101);
        int aggregator = 0;

        foreach (var providerConfig in providersConfiguration)
        {
            aggregator += providerConfig.Value;

            if (selection <= aggregator)
            {
                var provider = scope.ServiceProvider.GetKeyedService<ISmsProvider>(providerConfig.Key);

                if(provider is null)
                    throw new InvalidOperationException($"No such provider registered: {providerConfig.Key}");

                logger.LogInformation($"PercentProviderSelector: Provider: {providerConfig.Key}");
                return provider;
            }
        }

        throw new UnreachableException("Can not generate provider");
    }
}