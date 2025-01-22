using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace LMS.Blazor.Client.Services;

public class ClientApiService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager) : IApiService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient("BffClient");

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


    public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
    {
        Console.WriteLine($"Calling GetAsync with endpoint: {endpoint}");
        return await CallApiAsync<object?, TResponse>(
            endpoint,
            HttpMethod.Get,
            null
        );
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest dto)
    {
        return await CallApiAsync<TRequest, TResponse>(
            endpoint,
            HttpMethod.Post,
            dto
        );
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(
    string endpoint,
    TRequest dto)
    {
        return await CallApiAsync<TRequest, TResponse>(
            endpoint,
            HttpMethod.Put,
            dto
        );
    }

    public async Task PostAsync(string endpoint, HttpContent content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"proxy-endpoint/{endpoint}")
        {
            Content = content
        };

        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
           || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode();
    }

    public async Task<HttpResponseMessage> GetFileAsync(string endpoint)
    {
        Console.WriteLine($"Calling GetFileAsync with endpoint: {endpoint}");
        var request = new HttpRequestMessage(HttpMethod.Get, $"proxy-endpoint/{endpoint}");
        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
           || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode();
        return response;
    }


    private async Task<TResponse?> CallApiAsync<TRequest, TResponse>(string endpoint, HttpMethod httpMethod, TRequest? dto)
    {
        Console.WriteLine($"Calling CallApiAsync with endpoint: {endpoint} and method: {httpMethod}");
        var request = new HttpRequestMessage(httpMethod, $"proxy-endpoint/{endpoint}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (httpMethod != HttpMethod.Get && dto is not null)
        {
            var serialized = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(serialized);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        var response = await httpClient.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
           || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Handle 401 Unauthorized error
            Console.Error.WriteLine("Unauthorized access. Please check your credentials.");

            return default;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Handle 404 Not Found error
            Console.Error.WriteLine("Resource not found.");
            return default;
        }

        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength == 0 || response.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        var res = await JsonSerializer.DeserializeAsync<TResponse>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions, CancellationToken.None);
        return res;
    }

    // Used when no ResponseBody is needed (for NoContent response from API etc) and only a boolean for verification is needed.
    public async Task<bool> PutAsync<TRequest>(string endpoint, TRequest? dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"proxy-endpoint/{endpoint}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (dto is not null)
        {
            var serialized = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(serialized);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        var response = await httpClient.SendAsync(request);

        return response.IsSuccessStatusCode;
    }
}