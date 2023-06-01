﻿using ITLearning.Domain.Models;

namespace ITLearning.Course.Core.Contracts;

public interface ICourseItemFetcher
{
    public Task<ICourseItem> GetByItemId(long itemId);
}