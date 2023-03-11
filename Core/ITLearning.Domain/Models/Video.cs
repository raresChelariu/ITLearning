namespace ITLearning.Domain.Models;

public class Video
{
    public long Id { get; init; }
    public byte[] Content { get; init; }
    public string Name { get; init; }
}