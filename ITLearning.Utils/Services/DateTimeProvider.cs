using ITLearning.Utils.Contracts;

namespace ITLearning.Utils.Services;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}