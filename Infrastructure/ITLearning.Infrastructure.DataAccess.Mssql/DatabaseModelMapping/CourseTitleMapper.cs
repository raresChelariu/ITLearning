using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModelMapping;

public static class CourseTitleMapperExtensions
{
    public static CourseTitle ToCourseTitle(this CourseTitleDbDto dto)
    {
        return new CourseTitle
        {
            ItemId = dto.ItemId,
            ItemTitle = dto.ItemTitle,
            ItemType = (ItemType) dto.ItemTypeId
        };
    }
}