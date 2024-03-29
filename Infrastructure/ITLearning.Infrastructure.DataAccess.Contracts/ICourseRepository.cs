﻿using ITLearning.Domain;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ICourseRepository
{
    Task<long> Insert(Course course);
    Task<IEnumerable<Course>> GetByAuthorId(long authorId);
    Task<IEnumerable<Course>> GetAll();
    Task<Course> GetByAuthorIdAndCourseId(long authorId, long courseId);
    Task<Course> GetById(long courseId);

    Task<NextItemIdResult> GetNextItemId(long courseId, long itemId);

    Task UpdateUserCourseProgress(long userId, long courseId, long itemId);

    Task<IEnumerable<Course>> GetSqlCoursesByUserId(long userId);
    Task<long> GetUsersLastItemId(long userId, long courseId);
    Task ResetUserProgress(long userId, long courseId);
    Task<IEnumerable<Course>> GetByStudentId(long userId);
}