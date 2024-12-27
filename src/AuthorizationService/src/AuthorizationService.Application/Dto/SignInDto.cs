namespace AuthorizationService.Application.Dto;

public class SignInDto
{
   public required string Email { get; set; }
   public required string Password { get; set; }
   public required string Device {get; set;} 
};
