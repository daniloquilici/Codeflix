using Xunit;
using FluentAssertions;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryInputValidatorTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
        [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
        public void DontValidateWhenEmptyGuid()
        {
            //Arrange
            var input = _fixture.GetValidInput(Guid.Empty);
            var validator = new UseCase.UpdateCategoryInputValidator();

            //Act
            var validadeResult = validator.Validate(input);

            //Assert
            validadeResult.Should().NotBeNull();
            validadeResult.IsValid.Should().BeFalse();
            validadeResult.Errors.Should().HaveCount(1);
            validadeResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }

        [Fact(DisplayName = nameof(ValidateWhenValid))]
        [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
        public void ValidateWhenValid()
        {
            //Arrange
            var input = _fixture.GetValidInput();
            var validator = new UseCase.UpdateCategoryInputValidator();

            //Act
            var validadeResult = validator.Validate(input);

            //Assert
            validadeResult.Should().NotBeNull();
            validadeResult.IsValid.Should().BeTrue();
            validadeResult.Errors.Should().HaveCount(0);
        }
    }
}
