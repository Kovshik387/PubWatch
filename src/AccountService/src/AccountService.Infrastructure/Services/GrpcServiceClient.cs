using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AccountService.Infrastructure.Settings;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StorageServiceProto;

namespace AccountService.Infrastructure.Services;

public class GrpcServiceClient : IServiceClient
{
    private readonly GrpcEndPointRoute _endpointRoute;
    private readonly ILogger<GrpcServiceClient> _logger;
    
    public GrpcServiceClient(IOptions<GrpcEndPointRoute> endpointRoute, ILogger<GrpcServiceClient> logger)
    {
        _logger = logger;
        _endpointRoute = endpointRoute.Value;
    }
    
    public async Task<string> SendAddImageAsync<TData>(ImageDto imageDto)
    {
        using var channel = GrpcChannel.ForAddress(_endpointRoute.Url);
        var client = new StorageService.StorageServiceClient(channel);

        var response = await client.UploadImageAsync(new UploadImageRequest()
        {
            Image = ByteString.CopyFrom(imageDto.Image),
            ImageFormat = imageDto.ImageFormat,
            UserId = imageDto.AccountId
        });
        
        return response.Url;
    }

    public async Task<string?> GetPresignedImageUrlAsync(string userId)
    {
        using var channel = GrpcChannel.ForAddress(_endpointRoute.Url);
        var client = new StorageService.StorageServiceClient(channel);
        try
        {
            var response = await client.GetImageAsync(new GetImageRequest()
            {
                UserId = userId
            });
            return response.Url;
        }
        catch (RpcException e)
        {
            _logger.LogError(e, "Error while getting presigned image");
            return null;
        }
    }
}