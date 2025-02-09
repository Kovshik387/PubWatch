using AuthorizationService.Application.Dto;
using AuthorizationService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Api.Controllers;
[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost]
    [Route("signIn")]
    [AllowAnonymous]
    [ProducesResponseType(type:typeof(AuthDto), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
    {
        signInDto.Device = Request.Headers.UserAgent.ToString();
        return Ok(await _authService.SignInAsync(signInDto));
    }
    
    [HttpPost]
    [Route("signUp")]
    [AllowAnonymous]
    [ProducesResponseType(type:typeof(AuthDto), statusCode: StatusCodes.Status201Created)]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
    {
        signUpDto.Device = Request.Headers.UserAgent.ToString();
        return Ok(await _authService.SignUpAsync(signUpDto));
    }

    [HttpPost]
    [Route("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(type:typeof(AuthDto), statusCode: StatusCodes.Status201Created)]
    public async Task<IActionResult> RefreshAccess(string refreshToken)
    {
        return Ok(await _authService.GetAccessTokenAsync(refreshToken));
    }
    
    [HttpDelete]
    [Route("logout")]
    [Authorize]
    [ProducesResponseType(type:typeof(bool), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(string refreshToken)
    {
        return Ok(await _authService.Logout(refreshToken));
    }
    
    [HttpDelete]
    [Route("logout-all")]
    [Authorize]
    [ProducesResponseType(type:typeof(bool), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutAll(string refreshToken)
    {
        return Ok(await _authService.LogoutAll(refreshToken));
    }
    
}