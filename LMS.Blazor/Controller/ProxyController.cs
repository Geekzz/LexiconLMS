using LMS.Blazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LMS.Blazor.Controller;

[Route("proxy-endpoint")]
[ApiController]
public class ProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenStorage _tokenService;
    private readonly ILogger<ProxyController> _logger;

    public ProxyController(IHttpClientFactory httpClientFactory, ITokenStorage tokenService, ILogger<ProxyController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
        _logger = logger;
    }


    [Route("{*resource}")]
    [Authorize]
    public async Task<IActionResult> Proxy(string resource)
        {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Usermanager can be used here! 

        if (userId == null)
        {
            _logger.LogWarning("Unauthorized request: User ID is missing.");
            return Unauthorized();
        }
        _logger.LogInformation("Received request for resource: {Resource} with user ID: {UserId}", resource, userId);
        string endpoint = $"api/{resource}";

        if (resource == "courseForUser")
        {
            endpoint = "api/courses/user";
        }

        if (resource == "userInfo")
        {
            endpoint = $"api/users/{userId}";
        }

        var accessToken = await _tokenService.GetAccessTokenAsync(userId);
        //ToDo: Before continue look for expired accesstoken and call refresh enpoint instead.
        //Better with delegatinghandler or separate service to extract this logic!

        if (string.IsNullOrEmpty(accessToken))
        {
            _logger.LogWarning("Unauthorized request: Access token is missing or expired.");
            return Unauthorized();
        }
        _logger.LogInformation("Successfully retrieved access token for user: {UserId}", userId);

        var client = _httpClientFactory.CreateClient("LmsAPIClient");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var targetUri = new Uri($"{client.BaseAddress}{endpoint}{Request.QueryString}");
        var method = new HttpMethod(Request.Method);
        var requestMessage = new HttpRequestMessage(method, targetUri);

        // Add Console.WriteLine to log Content-Type and Content-Length
        _logger.LogInformation("Request Content-Type: {ContentType}", Request.ContentType);
        _logger.LogInformation("Request Content-Length: {ContentLength}", Request.ContentLength);

        // Handle request body if needed
        if (method != HttpMethod.Get && Request.ContentLength > 0)
        {

            requestMessage.Content = new StreamContent(Request.Body);

            if (!string.IsNullOrWhiteSpace(Request.ContentType))
            {
                requestMessage.Content.Headers.ContentType
                    = MediaTypeHeaderValue.Parse(Request.ContentType);
            }
        }
        // Forward request headers
        foreach (var header in Request.Headers)
        {
                if (!StringValues.IsNullOrEmpty(header.Value))
                {
                    _logger.LogInformation("Header: {Key} - {Value}", header.Key, string.Join(", ", header.Value));
                }
                if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
        }
        // Log the target URI
        _logger.LogInformation("Sending request to {TargetUri} with method {Method}", targetUri, method);

        try
        {
            // Send the request and get the response
            var response = await client.SendAsync(requestMessage);

            // Log response status code
            _logger.LogInformation("Received response from target API with status code: {StatusCode}", response.StatusCode);

            // Log response headers
            foreach (var header in response.Headers)
            {
                _logger.LogInformation("Response Header: {Header} - {Value}", header.Key, string.Join(", ", header.Value));
            }

            // Copy all response headers from the proxied response to the current response
            foreach (var header in response.Headers)
            {
                Response.Headers[header.Key] = header.Value.ToArray();
            }
            foreach (var header in response.Content.Headers)
            {
                Response.Headers[header.Key] = header.Value.ToArray();
            }

            // Set the status code and content type for the response
            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            // Copy the content of the response body
            var stream = await response.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(Response.Body);

            _logger.LogInformation("Successfully forwarded the response from target API.");
            return new EmptyResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while proxying the request to {TargetUri}", targetUri);
            return StatusCode(500, "Internal Server Error");
        }

    }
}