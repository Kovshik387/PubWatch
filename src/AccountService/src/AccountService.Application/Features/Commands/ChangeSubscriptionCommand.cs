using AccountService.Application.Interfaces;
using AccountService.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Commands;

public record ChangeSubscriptionCommand(Guid Id) : IRequest;

public class ChangeSubscriptionCommandHandler : IRequestHandler<ChangeSubscriptionCommand>
{
    private readonly IDbContext _dbContext;

    public ChangeSubscriptionCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ChangeSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id),
            cancellationToken: cancellationToken);
        
        if (account is null) throw new UserNotFoundException("User not found");
        
        account.Accept = !account.Accept; await _dbContext.SaveChangesAsync(cancellationToken);
    }
}