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

    public async Task<HttpResponseMessage> PostFileAsync(string endpoint, IBrowserFile browserFile, int courseId)
    {
        Console.WriteLine($"Calling PostFileAsync with endpoint: {endpoint} and courseId: {courseId}");
        // Log file details
        Console.WriteLine($"File Name: {browserFile.Name}");
        Console.WriteLine($"File Size: {browserFile.Size}");
        Console.WriteLine($"File Content Type: {browserFile.ContentType}");

        // Create a MultipartFormDataContent to hold the file
        var content = new MultipartFormDataContent();
        var fileStream = browserFile.OpenReadStream(maxAllowedSize: 10485760); // 10MB size limit (adjust as needed)
        // Check if the file stream is valid
        if (fileStream == null)
        {
            throw new InvalidOperationException("Failed to open file stream.");
        }
        //// Log the first few bytes of the file stream
        //var buffer = new byte[16];
        //var bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length);
        //Console.WriteLine($"First {bytesRead} bytes of file: {BitConverter.ToString(buffer, 0, bytesRead)}");

        //// Reset the stream position to the beginning
        //fileStream.Position = 0;

        // Add the file to the MultipartFormDataContent
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(browserFile.ContentType);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = browserFile.Name
        };
        content.Add(fileContent, "file", browserFile.Name);
        Console.WriteLine($"File Content Headers: {string.Join(", ", fileContent.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

        // Construct the full endpoint URL with query parameter
        var urlWithQuery = $"proxy-endpoint/{endpoint}?courseId={courseId}";

        // Create the request
        var request = new HttpRequestMessage(HttpMethod.Post, urlWithQuery)
        {
            Content = content
        };

        // Log the request details
        Console.WriteLine($"Request URL: {request.RequestUri}");
        Console.WriteLine($"Request Method: {request.Method}");
        Console.WriteLine($"Request Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
        Console.WriteLine($"Request Content Headers: {string.Join(", ", request.Content.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

        // Send the request
        var response = await httpClient.SendAsync(request);

        // Log the response details
        Console.WriteLine($"Response Status Code: {response.StatusCode}");
        Console.WriteLine($"Response Headers: {response.Headers}");

        // Handle errors
        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
           || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not successful.
        return response;
    }

    //public async Task PostFileAsync(string endpoint, IBrowserFile file, IDictionary<string, string>? additionalData = null)
    //{
    //    if (file == null || file.Size == 0)
    //        throw new ArgumentException("File is null or empty.", nameof(file));

    //    using var content = new MultipartFormDataContent();

    //    // Add the file content
    //    var fileStream = file.OpenReadStream(10_000_000); // 10 MB
    //    var fileContent = new StreamContent(fileStream);
    //    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

    //    // Set Content-Disposition header
    //    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
    //    {
    //        FileName = file.Name,
    //        DispositionType = "attachment"
    //    };

    //    content.Add(fileContent, "file", file.Name);

    //    //content.Add(new StreamContent(fileStream));


    //    // Add any additional form fields if needed
    //    if (additionalData != null)
    //    {
    //        foreach (var (key, value) in additionalData)
    //        {
    //            content.Add(new StringContent(value), key);
    //        }
    //    }

    //    var response = await httpClient.PostAsync($"proxy-endpoint/{endpoint}", content);

    //    if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
    //    {
    //        navigationManager.NavigateTo("AccessDenied");
    //        return;
    //    }

    //    response.EnsureSuccessStatusCode();
    //}


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