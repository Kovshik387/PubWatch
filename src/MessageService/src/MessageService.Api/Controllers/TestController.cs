using MediatR;
using MessageService.Application.Features;
using MessageService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Api.Controllers;

[ApiController]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // [HttpGet("api/test")]
    // public async Task<IActionResult> Get()
    // {
    //     await _mediator.Send(new SendNotificationCommand(
    //         new Account("fantokin03@mail.ru"),
    //         [new Currency("LOL","32"), new Currency("dsadsa","2"),new Currency("das;ldas;dasda","45343")],DateTime.Now.ToLongDateString())
    //     );
    //     return Ok();
    // }
}