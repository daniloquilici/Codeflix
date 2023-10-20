﻿using FluentValidation;

namespace quilici.Codeflix.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
    {
        public UpdateCategoryInputValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
