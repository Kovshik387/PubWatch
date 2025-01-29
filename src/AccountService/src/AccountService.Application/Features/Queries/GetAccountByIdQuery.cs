using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Queries;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto?>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?> 
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
        
        return account is null ? null : _mapper.Map<AccountDto>(account);
    }
}