using Sms.Application.Models;

namespace Sms.Application.InfrastructureServices;

public interface ISmsProvider
{
    Task SendSmsAsync(SmsDto sms);
}