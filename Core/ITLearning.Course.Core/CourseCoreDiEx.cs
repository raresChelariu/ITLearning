using ITLearning.Course.Core.Contracts;
using ITLearning.Course.Core.Interfaces;
using ITLearning.Course.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ITLearning.Course.Core;

public static class CourseCoreDiEx
{
    public static IServiceCollection AddCourseCore(this IServiceCollection services)
    {
        services.AddSingleton<ICourseItemFetcher, CourseItemFetcher>();
        services.AddSingleton<IItemFetcherStrategyCollection, ItemFetcherStrategyCollection>();
        services.AddSingleton<IQuizChoiceValidator, QuizChoiceValidator>();
        services.AddSingleton<ICourseProgressService, CourseProgressService>();
        services.AddSingleton<ISqlDatabaseBuilder, SqlDatabaseBuilder>();
        services.AddSingleton<ICourseDatabaseRunner, CourseDatabaseRunner>();
        services.AddSingleton<ISqlQuizAnswerValidator, SqlQuizAnswerValidator>();
        services.AddSingleton<ICourseStepProgressService, CourseStepProgressService>();
        return services;
    }
}