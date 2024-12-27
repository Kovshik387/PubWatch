using Google.Protobuf;
using Grpc.Net.Client;
using AccountService.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Commands;

public record AddProfileImageCommand(Guid Id, IFormFile File) : IRequest<string>;

public class AddProfileImageCommandHandler : IRequestHandler<AddProfileImageCommand, string>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;

    public AddProfileImageCommandHandler(IDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(AddProfileImageCommand request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        //TODO логика взаимодействия с storage-service

        using var client = GrpcChannel.ForAddress($"");
        
        await using MemoryStream memoryStream = new();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        
        throw new NotImplementedException();
    }
}

