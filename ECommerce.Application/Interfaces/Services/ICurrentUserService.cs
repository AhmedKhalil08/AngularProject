using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Services
{
   public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        string? Role { get; }
    }
}
