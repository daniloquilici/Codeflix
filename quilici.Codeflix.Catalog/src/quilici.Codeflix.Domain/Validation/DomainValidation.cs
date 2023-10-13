﻿using quilici.Codeflix.Domain.Exceptions;

namespace quilici.Codeflix.Domain.Validation
{
    public class DomainValidation
    {
        public static void NotNull(object? target, string fieldName)
        {
            if (target is null)
                throw new EntityValidationException($"{fieldName} should not be null");
        }

        public static void NotNullOrEmpty(string? target, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(target))
                throw new EntityValidationException($"{fieldName} should not be null or empty");
        }
    }
}
