using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sms.Application.Commands;
using Sms.Application.Models;

namespace Sms.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public sealed class SmsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendSms(SmsDto sms)
    {
        await sender.Send((SendSmsCommand)sms);
        return Ok();
    }
}