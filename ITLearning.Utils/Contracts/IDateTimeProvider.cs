namespace ITLearning.Utils.Contracts;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}