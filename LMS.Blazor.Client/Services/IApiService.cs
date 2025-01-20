using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<T> CallApiGetAsync<T>(string endpoint);
    Task<T> CallApiPatchAsync<T>(string endpoint, T data);
    Task<T> CallApiPutAsync<T>(string endpoint, T data);
}
