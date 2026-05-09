using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string? UserId =>
            _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? Email =>
            _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? Role =>
            _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }
}
