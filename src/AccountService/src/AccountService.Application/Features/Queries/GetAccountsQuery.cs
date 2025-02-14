using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Queries;

public record GetAccountsQuery() : IRequest<List<AccountEmailDto>>;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery,List<AccountEmailDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetAccountsQueryHandler(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<AccountEmailDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<AccountEmailDto>>(
            await _dbContext.Users.Where(x => x.Accept).ToListAsync(cancellationToken: cancellationToken));
    }
}       
    

