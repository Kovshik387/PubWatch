using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AccountService.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Commands;

public record DeleteChosenVoluteCommand(Guid Id, FavoriteDto FavoriteDto) : IRequest;

public class DeleteChosenVoluteCommandHandler : IRequestHandler<DeleteChosenVoluteCommand>
{
    private readonly IDbContext _dbContext;

    public DeleteChosenVoluteCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Handle(DeleteChosenVoluteCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (account is null) throw new UserNotFoundException("Account not found");
        
        var volute = await _dbContext.Favorites
            .FirstOrDefaultAsync(x => x.Iduser.Equals(request.Id) 
                                      && x.Volute.Equals(request.FavoriteDto.Volute), cancellationToken);
        
        if (volute is null) throw new VoluteNotFoundException("Volute not found");

        account.Favorites.Remove(volute); await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
