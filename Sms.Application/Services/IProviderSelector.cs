using Sms.Application.InfrastructureServices;

namespace Sms.Application.Services;

public interface IProviderSelector
{
    ISmsProvider GetSmsProvider();
}