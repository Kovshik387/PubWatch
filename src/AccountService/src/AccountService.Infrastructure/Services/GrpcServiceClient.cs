using AccountService.Application.Dto;
using AccountService.Application.Interfaces;
using AccountService.Infrastructure.Settings;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using StorageServiceProto;

namespace AccountService.Infrastructure.Services;

public class GrpcServiceClient : IServiceClient
{
    private readonly GrpcEndPointRoute _endpointRoute;

    public GrpcServiceClient(IOptions<GrpcEndPointRoute> endpointRoute)
    {
        _endpointRoute = endpointRoute.Value;
    }
    
    public async Task<string> SendAddImageAsync<TData>(ImageDto imageDto)
    {
        using var channel = GrpcChannel.ForAddress(_endpointRoute.StorageUrl);
        var client = new StorageService.StorageServiceClient(channel);

        var response = await client.UploadImageAsync(new UploadImageRequest()
        {
            Image = ByteString.CopyFrom(imageDto.Image),
            ImageFormat = imageDto.ImageFormat,
            UserId = imageDto.AccountId
        });
        
        return response.Url;
    }
    
    
}