using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using AccountService.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountService.Application.Features.Commands;

public record AddChosenVoluteCommand(Guid Id, FavoriteDto FavoriteDto) : IRequest;

public class AddChosenVoluteCommandHandler : IRequestHandler<AddChosenVoluteCommand> 
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<AddAccountCommandHandler> _logger;
    private readonly IMapper _mapper;
    
    public AddChosenVoluteCommandHandler(IDbContext dbContext, ILogger<AddAccountCommandHandler> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Handle(AddChosenVoluteCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (user is null) throw new UserNotFoundException($"User: {request.Id} not found");

        if (user.Favorites
                .FirstOrDefault(x => x.Volute.Equals(request.FavoriteDto.Volute)) is not null)
        {
            return;
        }
        
        user.Favorites.Add(_mapper.Map<Favorite>(request.FavoriteDto)); 
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}