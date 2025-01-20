using LMS.Blazor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LMS.Blazor.Controller;

[Route("proxy-endpoint")]
[ApiController]
public class ProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenStorage _tokenService;

    public ProxyController(IHttpClientFactory httpClientFactory, ITokenStorage tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
    }

    // ska man ha {*resource} på dessa? 
    [HttpGet("{*resource}")]
    [HttpPut("{*resource}")]
    [HttpPatch("{*resource}")]
    // [HttpPost("{*resource}")]
    // [HttpDelete("{*resource}")] 
    public async Task<IActionResult> Proxy(string resource) //ToDo query?
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Usermanager can be used here! 

        if (userId == null)
            return Unauthorized();

        string endpoint = $"api/{resource}";
        var accessToken = await _tokenService.GetAccessTokenAsync(userId);

        //Endpoint to Get course for logged in user, the CourseController in
        //the API will get the userId, no need to pass it in a querystring.
        if (resource == "courseForUser")
        {
            endpoint = "api/courses/user";
        }

        if ( resource == "userInfo")
        {
            endpoint = $"api/users/{userId}";
        }

        //ToDo: Before continue look for expired accesstoken and call refresh enpoint instead.
        //Better with delegatinghandler or separate service to extract this logic!

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized();
        }
        var client = _httpClientFactory.CreateClient("LmsAPIClient");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var targetUri = new Uri($"{client.BaseAddress}{endpoint}{Request.QueryString}");
        var method = new HttpMethod(Request.Method);
        var requestMessage = new HttpRequestMessage(method, targetUri); // testa kolla requestMessage, hur den ser ut, så vi vet vad api tar emot

        //Handles POST, detta bör funka med put osv med va? mm
        // jo, det är väl bara att den används för alla methods med req body (inte get).

        if (method != HttpMethod.Get && Request.ContentLength > 0)
        {
            requestMessage.Content = new StreamContent(Request.Body);
        }

        foreach (var header in Request.Headers)
        {
            if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }


        var response = await client.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
            return Unauthorized();

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
