﻿using Microsoft.Extensions.Logging;
using Sms.Application.InfrastructureServices;
using Sms.Application.Models;

namespace Sms.Infrastructure.InfrasturctureServices.SmsProviders;

public sealed class GeocellSmsProvider(ILogger<GeocellSmsProvider> logger) : ISmsProvider
{
    public Task SendSmsAsync(SmsDto sms)
    {
        logger.LogInformation("Sending sms message");
        return Task.CompletedTask;
    }
}