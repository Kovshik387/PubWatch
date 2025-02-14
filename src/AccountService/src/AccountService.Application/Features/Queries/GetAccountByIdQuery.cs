using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AccountService.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Queries;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto?>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?> 
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IServiceClient _serviceClient;
    
    public GetAccountByIdQueryHandler(IDbContext dbContext, IMapper mapper, IServiceClient serviceClient)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _serviceClient = serviceClient;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Users.FirstOrDefaultAsync(
            x => x.Id.Equals(request.Id), cancellationToken);

        if (account is null) throw new UserNotFoundException("Account not found");
        
        var imageUrl = await _serviceClient.GetPresignedImageUrlAsync(request.Id.ToString());
        
        var result = _mapper.Map<AccountDto>(account);
        result.Image = imageUrl ?? string.Empty;
        
        return result;
    }
}