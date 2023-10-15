using FluentAssertions;
using quilici.Codeflix.Domain.Exceptions;
using Xunit;
using DomainEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.UnitTest.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {
        private readonly CategoryTestFixture _categoryTestFixture;

        public CategoryTest(CategoryTestFixture categoryTestFixture)
        {
            this._categoryTestFixture = categoryTestFixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            //Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Act
            var dateBefore = DateTime.Now;
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var dateAfter = DateTime.Now.AddSeconds(1);

            //Assert

            //Usando fluentAssertions
            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreateAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreateAt >= dateBefore).Should().BeTrue();
            (category.CreateAt <= dateAfter).Should().BeTrue();
            (category.IsActive).Should().BeTrue();

            Assert.NotNull(category);
            Assert.Equal(validCategory.Name, category.Name);
            Assert.Equal(validCategory.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreateAt);
            Assert.True(category.CreateAt > dateBefore);
            Assert.True(category.CreateAt < dateAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            //Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Act
            var dateBefore = DateTime.Now;
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
            var dateAfter = DateTime.Now.AddSeconds(1);

            //Assert

            //Usando fluentAssertions
            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreateAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreateAt >= dateBefore).Should().BeTrue();
            (category.CreateAt <= dateAfter).Should().BeTrue();
            category.IsActive.Should().Be(isActive);

            Assert.NotNull(category);
            Assert.Equal(validCategory.Name, category.Name);
            Assert.Equal(validCategory.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreateAt);
            Assert.True(category.CreateAt > dateBefore);
            Assert.True(category.CreateAt < dateAfter);
            Assert.Equal(isActive, category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Usando fluentAssertions
            Action action =
                () => new DomainEntity.Category(name!, validCategory.Description);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");

            //Assert
            //Action action = () => new DomainEntity.Category(name!, "Category description");
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsEmpty()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Usando fluentAssertions
            Action action =
                () => new DomainEntity.Category(validCategory.Name, null!);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Description should not be null");

            //Assert
            //Action action = () => new DomainEntity.Category("Category name", null!);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Description should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesIsLessThan3Characters), parameters: 10)]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Usando fluentAssertions
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");

            //Assert
            //Action action = () => new DomainEntity.Category(invalidName, "Category description");
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");

            //Assert
            //Action action = () => new DomainEntity.Category(invalidName, "Category description");
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var invaliddescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(validCategory.Name, invaliddescription);
            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10000 characters long");

            //Assert
            //Action action = () => new DomainEntity.Category("Category name", invaliddescription);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            //Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
            category.Activate();

            category.IsActive.Should().BeTrue();

            //Assert
            //Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            //Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            //Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
            category.Deactivate();

            category.IsActive.Should().BeFalse();

            //Assert            
            //Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = _categoryTestFixture.GetValidCategory();
            var categoryNewValues = _categoryTestFixture.GetValidCategory();

            category.Update(categoryNewValues.Name, categoryNewValues.Description);

            category.Name.Should().Be(categoryNewValues.Name);
            category.Description.Should().Be(categoryNewValues.Description);

            //Assert
            //Assert.Equal(newValues.Name, category.Name);
            //Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = _categoryTestFixture.GetValidCategory();
            var newName = _categoryTestFixture.GetValidCategoryName();
            var currentDescription = category.Description;

            category.Update(newName);

            category.Name.Should().Be(newName);
            category.Description.Should().Be(currentDescription);

            //Assert
            //Assert.Equal(newValues.Name, category.Name);
            //Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = _categoryTestFixture.GetValidCategory();

            Action action = () => category.Update(name!);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");

            //Assert
            //Action action = () => category.Update(name!);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesIsLessThan3Characters), parameters: 10)]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var category = _categoryTestFixture.GetValidCategory();

            Action action = () => category.Update(invalidName);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at least 3 characters long");

            //Assert
            //Action action = () => category.Update(invalidName);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = _categoryTestFixture.GetValidCategory();
            var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

            Action action = () => category.Update(invalidName);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");

            //Assert
            //Action action = () => category.Update(invalidName);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var category = _categoryTestFixture.GetValidCategory();
            var invaliddescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

            while (invaliddescription.Length <= 10_000)
                invaliddescription += $"{invaliddescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

            Action action = () => category.Update("Category name", invaliddescription);
            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10000 characters long");

            //Assert
            //Action action = () => category.Update("Category name", invaliddescription);
            //var exception = Assert.Throws<EntityValidationException>(action);
            //Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }

        public static IEnumerable<object[]> GetNamesIsLessThan3Characters(int numberOfTests = 6)
        {
            var fixture = new CategoryTestFixture();

            for (int i = 0; i < numberOfTests; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[]
                {
                    fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
                };
            }
        }
    }
}
