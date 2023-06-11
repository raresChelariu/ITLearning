﻿using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ICourseScriptRepository
{
    Task<long> CreateScript(CourseScript courseScript);

    Task<IEnumerable<CourseScript>> GetScriptsByCourseId(long courseId);
}