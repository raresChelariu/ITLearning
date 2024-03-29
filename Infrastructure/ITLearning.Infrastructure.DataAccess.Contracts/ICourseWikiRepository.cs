using ITLearning.Domain;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ICourseWikiRepository
{
    Task<long> CreateWiki(CourseWiki wiki);
    
    Task<CourseWiki> GetWikiByItemId(long itemId);
}