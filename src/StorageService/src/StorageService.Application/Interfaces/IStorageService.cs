using StorageService.Application.Dto;

namespace StorageService.Application.Interfaces;

public interface IStorageService
{
    public Task<string> AddProfilePhotoAsync(AddProfileImageDto addProfileImageDto);
    public Task<bool> DeleteProfilePhotoAsync(string userId);
    public Task<string> GetProfilePhotoAsync(string userId);
}