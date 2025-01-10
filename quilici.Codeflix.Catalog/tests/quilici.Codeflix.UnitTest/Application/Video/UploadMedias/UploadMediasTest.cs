using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Video.UploadMedias;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Video.UploadMedias;
[Collection(nameof(UploadMediasTestFixture))]
public class UploadMediasTest
{
    private readonly UploadMediasTestFixture _fixture;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IVideoRepository> _videoRepositoryMock;
    private readonly Mock<IStorageService> _storageServiceMock;
    private readonly UseCase.UploadMedias _useCase;

    public UploadMediasTest(UploadMediasTestFixture fixture)
    {
        _fixture = fixture;
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _videoRepositoryMock = new Mock<IVideoRepository>();
        _storageServiceMock = new Mock<IStorageService>();
        _useCase = new UseCase.UploadMedias(_unitOfWorkMock.Object, _videoRepositoryMock.Object, _storageServiceMock.Object);
    }

    [Fact(DisplayName = nameof(UploadMedias))]
    [Trait("Application", "UploadMedias - Use Cases")]
    public async Task UploadMedias()
    {
        _videoRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(_fixture.GetValidVideo());
        _storageServiceMock.Setup(x => x.Upload(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())).ReturnsAsync(Guid.NewGuid().ToString());

        await _useCase.Handle(_fixture.GetValidInput(), CancellationToken.None);

        _videoRepositoryMock.VerifyAll();
        _storageServiceMock.Verify(x => x.Upload(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
