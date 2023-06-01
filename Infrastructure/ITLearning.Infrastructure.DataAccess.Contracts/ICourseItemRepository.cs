using ITLearning.Domain.Models;

namespace ITLearning.Infrastructure.DataAccess.Contracts;

public interface ICourseItemRepository
{
    public Task<ItemDetail> GetItemDetailById(long itemId);
    public Task<IEnumerable<ItemDetail>> GetItemDetailsByCourseId(long courseId);
}