using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<TResponse?> GetAsync<TResponse>(string endpoint);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest dto);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest dto);
    Task<bool> PutAsync<TRequest>(string endpoint, TRequest? dto);
    Task<HttpResponseMessage> GetFileAsync(string endpoint);
    Task<HttpResponseMessage> PostFileAsync(string endpoint, IBrowserFile browserFile, int courseId);
}