namespace Sms.Domain.Entities;

public sealed class Sms
{
    public required Guid Id { get; set; }
    public required string Message { get; set; }
    public required string Phone { get; set; }
    public required DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}