using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
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
    public async Task<HttpResponseMessage> PostFileAsync(string endpoint, IBrowserFile browserFile, int courseId)
    {
        var urlWithQuery = $"proxy-endpoint/{endpoint}?courseId={courseId}";
        const long maxAllowedSize = 10 * 1024 * 1024; // 10 MB
        
        Console.WriteLine($"Calling PostFileAsync with endpoint: {endpoint} and courseId: {courseId}");
        
        // Log file details
        Console.WriteLine($"File Name: {browserFile.Name}, File Size: {browserFile.Size}, ContentType: {browserFile.ContentType}");
        using var content = new MultipartFormDataContent();

        // Add file content
        var fileContent = new StreamContent(browserFile.OpenReadStream(maxAllowedSize));
        
        //fileContent.Headers.ContentType = new MediaTypeHeaderValue(browserFile.ContentType);
        fileContent.Headers.ContentType =
    new MediaTypeHeaderValue(string.IsNullOrEmpty(browserFile.ContentType) ? "application/octet-stream" : browserFile.ContentType);

        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = browserFile.Name
        };
        content.Add(fileContent, "file");

        // Send POST request
        var request = new HttpRequestMessage(HttpMethod.Post, urlWithQuery)
        {
            Content = content
        };

        // Log the request details
        Console.WriteLine($"Request URL: {request.RequestUri}");
        Console.WriteLine($"Request Method: {request.Method}");
        Console.WriteLine($"Request Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
        Console.WriteLine($"Request Content Headers: {string.Join(", ", request.Content.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
        
        return await httpClient.SendAsync(request);
    }

    private async Task<TResponse?> CallApiAsync<TRequest, TResponse>(string endpoint, HttpMethod httpMethod, TRequest? dto)
    {
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
            return default;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
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