using ITLearning.Utils.Contracts;
using ITLearning.Utils.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Utils;

public static class UtilsDiEx
{
    public static IServiceCollection AddUtils(this IServiceCollection services)
    {
        return services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}