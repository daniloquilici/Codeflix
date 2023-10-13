﻿using Bogus;
using FluentAssertions;
using quilici.Codeflix.Domain.Exceptions;
using quilici.Codeflix.Domain.Validation;
using Xunit;

namespace quilici.Codeflix.UnitTest.Domain.Validation
{
    public class DomainValidationTest
    {
        public Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();
            Action action = () => DomainValidation.NotNull(value, "FieldName");
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            Action action = () => DomainValidation.NotNull(value, "FieldName");
            action.Should().Throw<EntityValidationException>().WithMessage("FieldName should not be null");
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenNullOrEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void NotNullOrEmptyThrowWhenNullOrEmpty(string? target)
        {
            Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");
            action.Should().Throw<EntityValidationException>().WithMessage("fieldName should not be null or empty");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk() 
        {
            var target = Faker.Commerce.ProductName();
            Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");
            action.Should().NotThrow();
        }
    }
}
