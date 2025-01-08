using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Video.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Repository;
using System.Text;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.CreateVideo;

[Collection(nameof(CreateVideoTestFixture))]
public class CreateVideoTest
{
    private readonly CreateVideoTestFixture _fixture;

    public CreateVideoTest(CreateVideoTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(Create))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task Create()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
    }

    [Theory(DisplayName = nameof(CreateThrowWithInvalidInput))]
    [Trait("Application", "Create video - Uses Cases")]
    [ClassData(typeof(CreateVideoTestDataGenerator))]
    public async Task CreateThrowWithInvalidInput(CreateVideoInput input, string expectedValidationError)
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), Mock.Of<IStorageService>());

        var action = async () => await useCase.Handle(input, CancellationToken.None);
        var exceptionAssertion = await action.Should().ThrowAsync<EntityValidationException>();

        exceptionAssertion.WithMessage("There are validation errors").Which.Errors!.ToList()[0].Message.Should().Be(expectedValidationError);

        repositoryMock.Verify(x => x.Insert(It.IsAny<DomainEntity.Video>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = nameof(CreateWithCategoriesIds))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithCategoriesIds()
    {
        var videoRepositoryMock = new Mock<IVideoRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        var exampleCategories = Enumerable.Range(1,5).Select(_ => Guid.NewGuid()).ToList();
        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategories);


        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, videoRepositoryMock.Object, categoryRepositoryMock.Object, Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput(exampleCategories);

        var output = await useCase.Handle(input, CancellationToken.None);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.CategoriesIds.Should().BeEquivalentTo(exampleCategories);

        videoRepositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened &&
            video.Categories.All(category => exampleCategories.Contains(category))
            ), It.IsAny<CancellationToken>()), Times.Once);

        categoryRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(ThrowsWhenCategoryIdInvalid))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task ThrowsWhenCategoryIdInvalid()
    {
        var videoRepositoryMock = new Mock<IVideoRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var exampleCategories = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
        var removedItem = exampleCategories[2];
        categoryRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategories.FindAll(x => x != removedItem).ToList().AsReadOnly());

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, videoRepositoryMock.Object, categoryRepositoryMock.Object, Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput(exampleCategories);

        var action = async () =>  await useCase.Handle(input, CancellationToken.None);        
        
        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category Id not found: {removedItem}");
        categoryRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(CreateWithGenresIds))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithGenresIds()
    {
        var videoRepositoryMock = new Mock<IVideoRepository>();
        var categoryRepositoryMock = new Mock<ICategoryRepository>();
        var genreRepositoryMock = new Mock<IGenreRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var exampleIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
        genreRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleIds);

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, videoRepositoryMock.Object, categoryRepositoryMock.Object, genreRepositoryMock.Object, Mock.Of<ICastMemberRepository>(), Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput(genresIds: exampleIds);

        var output = await useCase.Handle(input, CancellationToken.None);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.CategoriesIds.Should().BeEmpty();
        output.GenresIds.Should().BeEquivalentTo(exampleIds);

        videoRepositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened &&
            video.Genres.All(id => exampleIds.Contains(id))
            ), It.IsAny<CancellationToken>()), Times.Once);

        genreRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(ThrowswhenInvalidGenresIds))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task ThrowswhenInvalidGenresIds()
    {
        var videoRepositoryMock = new Mock<IVideoRepository>();
        var genreRepositoryMock = new Mock<IGenreRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var exampleIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
        var removedItem = exampleIds[2];
        genreRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleIds.FindAll(x => x != removedItem));


        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, videoRepositoryMock.Object, Mock.Of<ICategoryRepository>(), genreRepositoryMock.Object, Mock.Of<ICastMemberRepository>(), Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput(genresIds: exampleIds);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related genre Id not found: {removedItem}");
        genreRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(CreateWithCastMembersIds))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithCastMembersIds()
    {
        var videoRepositoryMock = new Mock<IVideoRepository>();
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var exampleIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
        castMemberRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleIds);

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, videoRepositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), castMemberRepositoryMock.Object, Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput(castMembersIds: exampleIds);

        var output = await useCase.Handle(input, CancellationToken.None);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.CategoriesIds.Should().BeEmpty();
        output.GenresIds.Should().BeEmpty();
        output.CastMembersIds.Should().BeEquivalentTo(exampleIds);

        videoRepositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened &&
            video.CastMembers.All(id => exampleIds.Contains(id))
            ), It.IsAny<CancellationToken>()), Times.Once);

        castMemberRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(ThrowswhenInvalidGenresIds))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task ThrowswhenInvalidCastMemberIds()
    {
        var videoRepositoryMock = new Mock<IVideoRepository>();
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var exampleIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
        var removedItem = exampleIds[2];
        castMemberRepositoryMock.Setup(x => x.GetIdsListByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleIds.FindAll(x => x != removedItem));


        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, videoRepositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), castMemberRepositoryMock.Object, Mock.Of<IStorageService>());
        var input = _fixture.CreateValidInput(castMembersIds: exampleIds);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related castmember Id not found: {removedItem}");
        castMemberRepositoryMock.VerifyAll();
    }

    [Fact(DisplayName = nameof(CreateWithThumb))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithThumb()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var storageServiceMock = new Mock<IStorageService>();
        var expectedThumbName = "thumb.jpg";

        storageServiceMock.Setup(x => x.Upload(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedThumbName);

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), storageServiceMock.Object);
        var input = _fixture.CreateValidInput(thumb: _fixture.GetValidImageFileInput());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        storageServiceMock.VerifyAll();

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.Thumb.Should().Be(expectedThumbName);
    }

    [Fact(DisplayName = nameof(CreateWithBanner))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithBanner()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var storageServiceMock = new Mock<IStorageService>();
        var expectedBannerName = "banner.jpg";

        storageServiceMock.Setup(x => x.Upload(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedBannerName);

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), storageServiceMock.Object);
        var input = _fixture.CreateValidInput(banner: _fixture.GetValidImageFileInput());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        storageServiceMock.VerifyAll();

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.Banner.Should().Be(expectedBannerName);
    }

    [Fact(DisplayName = nameof(CreateWithThumbHalf))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithThumbHalf()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var storageServiceMock = new Mock<IStorageService>();
        var expectedThumbHalfName = "thumbhalf.jpg";

        storageServiceMock.Setup(x => x.Upload(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedThumbHalfName);

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), storageServiceMock.Object);
        var input = _fixture.CreateValidInput(thumbHalf: _fixture.GetValidImageFileInput());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        storageServiceMock.VerifyAll();

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.ThumbHalf.Should().Be(expectedThumbHalfName);
    }

    [Fact(DisplayName = nameof(CreateWithAllImages))]
    [Trait("Application", "Create video - Uses Cases")]
    public async Task CreateWithAllImages()
    {
        var repositoryMock = new Mock<IVideoRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var storageServiceMock = new Mock<IStorageService>();
        var expectedThumbHalfName = "thumbhalf.jpg";
        var expectedThumbName = "thumb.jpg";
        var expectedBannerName = "banner.jpg";

        storageServiceMock.Setup(x => x.Upload(It.Is<string>(x => x.EndsWith("-thumbhalf.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedThumbHalfName);
        storageServiceMock.Setup(x => x.Upload(It.Is<string>(x => x.EndsWith("-thumb.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedThumbName);
        storageServiceMock.Setup(x => x.Upload(It.Is<string>(x => x.EndsWith("-banner.jpg")), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedBannerName);

        var useCase = new UseCase.CreateVideo(unitOfWorkMock.Object, repositoryMock.Object, Mock.Of<ICategoryRepository>(), Mock.Of<IGenreRepository>(), Mock.Of<ICastMemberRepository>(), storageServiceMock.Object);
        var input = _fixture.CreateValidInputWithAllImages();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Insert(It.Is<DomainEntity.Video>(video =>
            video.Title == input.Title &&
            video.Published == input.Published &&
            video.Description == input.Description &&
            video.Duration == input.Duration &&
            video.Rating == input.Rating &&
            video.Id != Guid.Empty &&
            video.YearLaunched == input.YearLaunched &&
            video.Opened == input.Opened
            ), It.IsAny<CancellationToken>()), Times.Once);

        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        storageServiceMock.VerifyAll();

        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);
        output.Title.Should().Be(input.Title);
        output.Published.Should().Be(input.Published);
        output.Description.Should().Be(input.Description);
        output.Duration.Should().Be(input.Duration);
        output.Rating.Should().Be(input.Rating);
        output.YearLaunched.Should().Be(input.YearLaunched);
        output.Opened.Should().Be(input.Opened);
        output.ThumbHalf.Should().Be(expectedThumbHalfName);
        output.Thumb.Should().Be(expectedThumbName);
        output.Banner.Should().Be(expectedBannerName);
    }
}
