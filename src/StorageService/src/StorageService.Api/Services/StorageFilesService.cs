using Grpc.Core;
using StorageService.Application.Dto;
using StorageService.Application.Interfaces;
using StorageServiceProto;

namespace StorageService.Api.Services;

public class StorageFilesService : StorageServiceProto.StorageService.StorageServiceBase
{
    private readonly IStorageService _storageService;
    
    public StorageFilesService(IStorageService storageService)
    {
        _storageService = storageService;
    }
    
    public override async Task<StorageResponse> UploadImage(UploadImageRequest request, ServerCallContext context)
    {
        var response = await _storageService.AddProfilePhotoAsync(new
            AddProfileImageDto(request.UserId, request.Image.ToByteArray(), request.ImageFormat));
        
        return new StorageResponse()
        {
            Success = true,
            Url = response
        };
    }

    public override async Task<StorageResponse> DeleteImage(DeleteImageRequest request, ServerCallContext context)
    {
        var response = await _storageService.DeleteProfilePhotoAsync(request.UserId);
        return new StorageResponse()
        {
            Success = response
        };
    }

    public override async Task<StorageResponse> GetImage(GetImageRequest request, ServerCallContext context)
    {
        var response = await _storageService.GetProfilePhotoAsync(request.UserId);
        return new StorageResponse()
        {
            Success = true,
            Url = response
        };
    }
}