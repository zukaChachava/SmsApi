using Microsoft.Extensions.DependencyInjection;
using Sms.Application.InfrastructureServices;
using Sms.Infrastructure.InfrasturctureServices.SmsProviders;

namespace Sms.Infrastructure.Extensions;

public static class RegisterInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddKeyedSingleton<ISmsProvider, MagtiSmsProvider>("Magti");
        services.AddKeyedSingleton<ISmsProvider, GeocellSmsProvider>("Geocell");
        services.AddKeyedSingleton<ISmsProvider, TwilioSmsProvider>("Twilio");

        return services;
    }
}