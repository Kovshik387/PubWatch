using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using StorageService.Application.Dto;
using StorageService.Application.Interfaces;
using StorageService.Application.Settings;

namespace StorageService.Application.Services;

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;
    private readonly IAmazonS3 _s3Client;
    
    private readonly StorageSettings _storageSettings;
    
    public StorageService(ILogger<StorageService> logger, IAmazonS3 s3Client, IOptions<StorageSettings> storageSettings)
    {
        _logger = logger;
        _s3Client = s3Client;
        _storageSettings = storageSettings.Value;
    }
    
    public async Task<string> AddProfilePhotoAsync(AddProfileImageDto addProfileImageDto)
    {
        var bucketName = addProfileImageDto.Id;
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName))
        {
            _logger.LogInformation($"Creating bucket {bucketName}");
            await _s3Client.PutBucketAsync(new PutBucketRequest { BucketName = bucketName });
        }
        else
        {
            _logger.LogInformation($"Removing old object {addProfileImageDto.Id}");
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = addProfileImageDto.Id
            });
        }

        using var stream = new MemoryStream(addProfileImageDto.ImageBytes);
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = addProfileImageDto.Id,
            InputStream = stream,
            ContentType = addProfileImageDto.Format
        };

        var response = await _s3Client.PutObjectAsync(putRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? addProfileImageDto.Id : string.Empty;
    }

    public async Task<bool> DeleteProfilePhotoAsync(string userId)
    {
        var objectKey = userId ?? throw new ArgumentNullException(nameof(userId));

        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, userId))
        {
            _logger.LogInformation($"Bucket {userId} doesn't exist");
            return false;
        }

        await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = userId,
            Key = objectKey
        });

        return true;
    }

    public async Task<string> GetProfilePhotoAsync(string userId)
    {
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, userId))
            return "";
        
        var args = new GetPreSignedUrlRequest()
        {
            Expires = DateTime.Now.AddHours(1),
            Key = userId,
            BucketName = userId,
        };
        var result = await _s3Client.GetPreSignedURLAsync(args);
        
        return result.Replace(_storageSettings.PrivateEndPoint,_storageSettings.PublicEndPoint);
    }
}