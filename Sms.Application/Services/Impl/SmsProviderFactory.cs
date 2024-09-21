using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sms.Application.InfrastructureServices;
using Sms.Application.Options;

namespace Sms.Application.Services.Impl;

public sealed class SmsProviderFactory(
    IServiceScopeFactory serviceScopeFactory,
    IOptionsMonitor<ProviderSelectorOptions> selectionOptions
        ) : ISmsProviderFactory
{
    public ISmsProvider GetSmsProvider(string providerName)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetService<ISmsProvider>();

        if(provider is null)
            throw new NotSupportedException($"Provider {providerName} is not supported");

        return provider;
    }

    public ISmsProvider GetAnySmsProvider()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var selector = scope.ServiceProvider.GetKeyedService<IProviderSelector>(selectionOptions.CurrentValue.SelectorStrategy);

        if(selector is null)
            throw new NotSupportedException($"Selection strategy ({selector}) is not supported");

        return selector.GetSmsProvider();
    }
}