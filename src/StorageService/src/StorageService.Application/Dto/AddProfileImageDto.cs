namespace StorageService.Application.Dto;

public record AddProfileImageDto(string Id, byte[] ImageBytes, string Format);