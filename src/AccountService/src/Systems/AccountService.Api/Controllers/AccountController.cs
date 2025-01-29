using AccountService.Application.Dto;
using AccountService.Application.Features.Commands;
using AccountService.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers;

[ApiController]
[Route("/api/account")]
public class AccountController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<AccountController> _logger;
    
    public AccountController(IMediator mediator, ILogger<AccountController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("/api/account/{id}")]
    public async Task<ActionResult<AccountDto>> GetAccountDetailsAsync(Guid id)
    {
        return Ok(await _mediator.Send(new GetAccountByIdQuery(id)));
    }

    [HttpPost]
    [Authorize]
    [Route("/api/account/upload-image")]
    public async Task<ActionResult> UploadImageAsync([FromQuery] Guid id, IFormFile file)
    {
        return Ok(await _mediator.Send(new AddProfileImageCommand(id, file)));
    }

    [HttpPost]
    [Authorize]
    [Route("/api/account/add-volute")]
    public async Task<ActionResult> AddVoluteAsync(Guid id, [FromBody] FavoriteDto favorite)
    {
        await _mediator.Send(new AddChosenVoluteCommand(id, favorite)); return Ok();
    }
}