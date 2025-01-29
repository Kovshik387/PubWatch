using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Queries;

public record GetAccountsQuery() : IRequest<IEnumerable<AccountDto>>;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetAccountsQueryHandler(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<AccountDto>>(
            await _dbContext.Users.Where(x => x.Accept).ToListAsync(cancellationToken: cancellationToken));
    }
}       
    

