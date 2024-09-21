using Sms.Application.Commands;

namespace Sms.Application.Models;

public sealed record SmsDto(
    string Phone,
    string Message
    )
{
    public static implicit operator Domain.Entities.Sms(SmsDto smsDto)
    {
        return new Domain.Entities.Sms
        {
            Phone = smsDto.Phone,
            Message = smsDto.Message,
            CreatedAt = DateTime.UtcNow,
            Id = Guid.NewGuid()
        };
    }

    public static implicit operator SmsDto(Domain.Entities.Sms sms)
    {
        return new SmsDto(sms.Phone, sms.Message);
    }

    public static implicit operator SendSmsCommand(SmsDto smsDto)
    {
        return new SendSmsCommand(smsDto.Phone, smsDto.Message);
    }

    public static implicit operator SmsDto(SendSmsCommand command)
    {
        return new SmsDto(command.PhoneNumber, command.Message);
    }
}