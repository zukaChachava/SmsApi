namespace Sms.Application.Options;

public sealed class SmsProvidersOptions
{
    public required IDictionary<string, int> Providers { get; set; }
}