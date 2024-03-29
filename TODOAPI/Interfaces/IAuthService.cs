﻿using AccessData.Models;
using Microsoft.Identity.Client;
using TODOAPI.Models.Request;
using TODOAPI.Models.Response;

namespace TODOAPI.Interfaces
{
    public interface IAuthService
    {
        Task<(bool isError, ApplicationException? error, UserRegisterResponse? result)> RegisterAsync(RegisterUserRequest userRegistrationDto);
        Task<LoginResponse> LoginAsync(string email, string password);
        string GenerateJwtToken(User user);
    }
}
