using ECommerce.Application.DTOs.Auth;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECommerce.Domain.Entities;
using ECommerce.Application.DTOs;
//using ECommerce.Infrastructure.Services.MailConfService;


namespace ECommerce.Application.Interfaces.Services
{
   
    public interface IMailConfService
    {
        Task <Result<string>> ConfirmEmail(string userId, string token);
        Task<Result<string>> SendOrderStatusUpdateAsync(string userEmail, int orderId, string newStatus);
    }
}
