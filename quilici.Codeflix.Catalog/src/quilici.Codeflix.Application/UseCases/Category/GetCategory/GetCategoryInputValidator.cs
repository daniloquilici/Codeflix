﻿using FluentValidation;

namespace quilici.Codeflix.Application.UseCases.Category.GetCategory
{
    public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput>
    {
        public GetCategoryInputValidator() => RuleFor(x => x.Id).NotEmpty();
    }
}
