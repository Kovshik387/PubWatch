using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AutoMapper;
using Grpc.Net.Client;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Commands;

public record AddProfileImageCommand(Guid Id, IFormFile File) : IRequest<string>;

public class AddProfileImageCommandHandler : IRequestHandler<AddProfileImageCommand, string>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IServiceClient _serviceClient;
    
    public AddProfileImageCommandHandler(IDbContext dbContext, IMapper mapper, IServiceClient serviceClient)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _serviceClient = serviceClient;
    }

    public async Task<string> Handle(AddProfileImageCommand request, CancellationToken cancellationToken)
    {
        await using MemoryStream memoryStream = new();
        await request.File.CopyToAsync(memoryStream, cancellationToken);

        var client = await _serviceClient.SendAddImageAsync<string>(new ImageDto()
        {
            Image = memoryStream.ToArray(),
            ImageFormat = request.File.ContentType,
            AccountId = request.Id.ToString()
        });
        
        return client;
    }
}

