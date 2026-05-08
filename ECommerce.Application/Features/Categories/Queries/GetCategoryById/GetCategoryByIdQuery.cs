using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery: IRequest<CategoryDto>
    {
        public int Id { get; set; }
    }
}
