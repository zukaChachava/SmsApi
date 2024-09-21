using FluentValidation;
using MediatR;
using Sms.Application.Services;

namespace Sms.Application.Commands;

public sealed record SendSmsCommand(string PhoneNumber, string Message) : IRequest;


public sealed class SendSmsCommandHandler(ISmsProviderFactory smsProviderFactory) : IRequestHandler<SendSmsCommand>
{
    public Task Handle(SendSmsCommand request, CancellationToken cancellationToken)
    {
        return smsProviderFactory.GetAnySmsProvider().SendSmsAsync(request);
    }
}


public sealed class SendSmsCommandValidator : AbstractValidator<SendSmsCommand>
{
    public SendSmsCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().Length(3, 20);
        RuleFor(x => x.Message).NotEmpty().NotNull().Length(1, 500);
    }
}