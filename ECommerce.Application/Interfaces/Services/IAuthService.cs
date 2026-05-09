using ECommerce.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto model);
        Task<AuthResponseDto> RegisterSellerAsync(RegisterSellerDto model);
        Task<AuthResponseDto> LoginAsync(LoginDto model);
    }
}
