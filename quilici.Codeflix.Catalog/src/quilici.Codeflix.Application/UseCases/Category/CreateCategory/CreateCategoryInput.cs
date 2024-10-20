﻿using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryInput : IRequest<CategoryModelOutput>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public CreateCategoryInput(string name, string? description = null, bool isActive = true)
        {
            Name = name;
            Description = description ?? "";
            IsActive = isActive;
        }
    }
}
