﻿using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<T> CallApiGetAsync<T>(string endpoint);
}
