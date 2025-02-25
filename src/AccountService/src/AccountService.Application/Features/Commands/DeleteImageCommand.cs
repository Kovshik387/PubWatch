using AccountService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountService.Application.Features.Commands;

public record DeleteImageCommand(Guid Id) : IRequest;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
    private readonly IServiceClient _serviceClient;
    private readonly ILogger<DeleteImageCommandHandler> _logger;
    
    public DeleteImageCommandHandler(IServiceClient serviceClient, ILogger<DeleteImageCommandHandler> logger)
    {
        _serviceClient = serviceClient;
        _logger = logger;
    }

    public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteImageCommandHandler");
        var response =  await _serviceClient.DeleteImageAsync(request.Id);

        if (!response) throw new InvalidDataException("Image was deleted successfully");
    }
}