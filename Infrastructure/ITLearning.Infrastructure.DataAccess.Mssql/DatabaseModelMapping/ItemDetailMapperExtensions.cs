using ITLearning.Domain.Models;
using ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModels;

namespace ITLearning.Infrastructure.DataAccess.Mssql.DatabaseModelMapping;

internal static class ItemDetailMapperExtensions
{
    public static ItemDetail ToItemDetails(this ItemDetailsDbDto dto)
    {
        return new ItemDetail
        {
            ItemId = dto.ItemId,
            ItemTitle = dto.ItemTitle,
            ItemType = (ItemType) dto.ItemTypeId
        };
    }
}