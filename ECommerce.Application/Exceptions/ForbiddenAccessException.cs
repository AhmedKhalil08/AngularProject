using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
}
