using System.Text.Json;

namespace Sms.Api.Models;

public sealed record ApiError(int StatusCode, string Message)
{
    public override string ToString() => JsonSerializer.Serialize(this);
}