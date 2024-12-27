using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountService.Application.Commands;

public record AddAccountCommand(Guid Id, string Name,
    string Surname, string Patronymic, string Email) : IRequest<Guid>;

public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, Guid>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<AddAccountCommandHandler> _logger;
    
    public AddAccountCommandHandler(IDbContext dbContext, ILogger<AddAccountCommandHandler> logger)
    {
        _dbContext = dbContext; _logger = logger;
    }

    public async Task<Guid> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.Users.Add(new User()
            {
                Id = request.Id,
                Name = request.Name,
                Email = request.Email,
                Surname = request.Surname,
                Patronymic = request.Patronymic,
                Accept = false
            });
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        //TODO custom exception
        catch (Exception ex)
        {
            
        }
        _logger.LogInformation($"Add account command executed. {request.Id}");
        return request.Id;
    }
}