using Sms.Application.InfrastructureServices;

namespace Sms.Application.Services;

public interface ISmsProviderFactory
{
    ISmsProvider GetSmsProvider(string providerName);
    ISmsProvider GetAnySmsProvider();
}