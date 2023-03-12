namespace ITLearning.Domain.Models;

public class Video
{
    private readonly VideoEntry _entry = new();
    public long Id
    {
        get => _entry.Id;
        set => _entry.Id = value;
    }

    public byte[] Content { get; set; }

    public string Name
    {
        get => _entry.Name;
        set => _entry.Name = value;
    }
}