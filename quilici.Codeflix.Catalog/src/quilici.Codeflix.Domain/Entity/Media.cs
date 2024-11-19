using quilici.Codeflix.Catalog.Domain.Enum;

namespace quilici.Codeflix.Catalog.Domain.Entity;
public class Media
{
    public string FilePath { get; private set; }

    public string? EncodedPath { get; private set; }

    public MediaStatus Status { get; set; }

    public Media(string filePath)
    {
        FilePath = filePath.Trim();
        Status = MediaStatus.Pending;
    }

    public void UpdateAsSentToEncode()
    {
        Status = MediaStatus.Processing;
    }

    public void UpdateAsEncoded(string encondedExamplePath)
    {
        Status = MediaStatus.Completed;
        EncodedPath = encondedExamplePath;
    }
}
