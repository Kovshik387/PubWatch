using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using StorageService.Application.Dto;
using StorageService.Application.Interfaces;

namespace StorageService.Application.Services;

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;
    private readonly IMinioClientFactory _minioClientFactory;
    
    public StorageService(ILogger<StorageService> logger, IMinioClientFactory minioClientFactory)
    {
        _logger = logger;
        _minioClientFactory = minioClientFactory;
    }
    
    public async Task<string> AddProfilePhotoAsync(AddProfileImageDto addProfileImageDto)
    {
        using var minioClient = _minioClientFactory.CreateClient();
        
        if (!await minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(addProfileImageDto.Id)))
        {
            _logger.LogInformation($"Creating bucket {addProfileImageDto.Id}");
            await minioClient.MakeBucketAsync(new MakeBucketArgs()
                .WithBucket(addProfileImageDto.Id));
        }
        else
        {
            _logger.LogInformation($"Remove old object {addProfileImageDto.Id}");
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(addProfileImageDto.Id)
                .WithObject(addProfileImageDto.Id)
            );
        }
        
        var putObjectArgs = new PutObjectArgs()
                .WithBucket(addProfileImageDto.Id)
                .WithStreamData(new MemoryStream(addProfileImageDto.ImageBytes))
                .WithObject(addProfileImageDto.Id)
                .WithContentType(addProfileImageDto.Format)
                .WithObjectSize(addProfileImageDto.ImageBytes.Length)
            ;
        var response = await minioClient.PutObjectAsync(putObjectArgs);
        
        return response.ResponseContent;
    }

    public async Task<bool> DeleteProfilePhotoAsync(string userId)
    {
        using var minioClient = _minioClientFactory.CreateClient();

        if (!await minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(userId)))
        {
            _logger.LogInformation($"Bucket: {userId} doesn't exist");
            return false;
        }

        await minioClient.RemoveObjectsAsync(new RemoveObjectsArgs()
            .WithBucket(userId)
            .WithObject(userId));
        return true;
    }

    public async Task<string> GetProfilePhotoAsync(string userId)
    {
        using var minioClient = _minioClientFactory.CreateClient();

        if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(userId)))
            return "";
        
        //TODO configuration time
        var args = new PresignedGetObjectArgs()
                .WithBucket(userId)
                .WithObject(userId)
                .WithExpiry(60 * 60 * 60)
        ;
        
        return await minioClient.PresignedGetObjectAsync(args);
    }
}