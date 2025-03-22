﻿using UrlShortener.App.Shared.Dto;

namespace UrlShortener.App.Frontend.Business
{
    public interface IMappingsService
    {
        Task<List<UrlMappingDto>?> GetMappings();
        Task<CreateMappingResponseDto?> CreateMapping(string longUrl, string? name = null);
        Task<UserStatsDto?> GetStats();
        Task<bool> DeleteMapping(UrlMappingDto mapping);
    }
}
