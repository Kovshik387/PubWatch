using System.Collections;
using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AccountService.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Queries;

public record GetAccountsQuery() : IRequest<IEnumerable<UserDto>>;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, IEnumerable<UserDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetAccountsQueryHandler(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<UserDto>>(
            await _dbContext.Users.Where(x => x.Accept).ToListAsync(cancellationToken: cancellationToken));
    }
}       
    

