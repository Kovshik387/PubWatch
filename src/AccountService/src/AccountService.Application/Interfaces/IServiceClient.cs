using AccountService.Application.Dto;

namespace AccountService.Application.Interfaces;

public interface IServiceClient
{
    public Task<string> SendAddImageAsync<TData>(ImageDto imageDto);
    public Task<bool> DeleteImageAsync(Guid imageId);
    public Task<string?> GetPresignedImageUrlAsync(string userId);
}