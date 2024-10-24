﻿using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public class CreateGenreInput : IRequest<GenreModelOutput>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<Guid>? CategoriesIds { get; set; }

        public CreateGenreInput(string name, bool isActive, List<Guid>? categoriesIds = null)
        {
            Name = name;
            IsActive = isActive;
            CategoriesIds = categoriesIds;
        }
    }
}
