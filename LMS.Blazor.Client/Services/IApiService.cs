using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<List<T>> CallApiGetAllAsync<T>(string endpoint);
}
