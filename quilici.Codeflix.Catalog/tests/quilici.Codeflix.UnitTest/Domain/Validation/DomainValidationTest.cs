﻿using Bogus;
using FluentAssertions;
using Microsoft.VisualBasic;
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
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.NotNull(value, fieldName);
            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.NotNull(value, fieldName);
            action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be null");
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenNullOrEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void NotNullOrEmptyThrowWhenNullOrEmpty(string? target)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
            action.Should().Throw<EntityValidationException>().WithMessage($"{fieldName} should not be empty or null");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk() 
        {
            var target = Faker.Commerce.ProductName();
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
        public void MinLengthThrowWhenLess(string target, int minLength) 
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
            action.Should().Throw<EntityValidationException>($"{fieldName} should be at leats {minLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
        {
            yield return new object[] { "123456", 10 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests-1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(1, 20);
                yield return new object[] { example, minLength };
            }    
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
        public void MinLengthOk(string target, int minLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
        {
            yield return new object[] { "123456", 6 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests-1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random()).Next(1, 5);
                yield return new object[] { example, minLength };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
            action.Should().Throw<EntityValidationException>($"{fieldName} should be less or equal {maxLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 5) 
        {
            yield return new object[] { "123456", 5 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random()).Next(1, 5);
                yield return new object[] { example, minLength };
            }
        }

        [Theory(DisplayName = nameof(MaxLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
        public void MaxLengthOk(string target, int maxLength)
        {
            var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
        {
            yield return new object[] { "123456", 6 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(0, 5);
                yield return new object[] { example, minLength };
            }
        }
    }
}
