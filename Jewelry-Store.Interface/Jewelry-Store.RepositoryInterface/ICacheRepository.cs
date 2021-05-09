using Jewelry_Store.Common;
using System;

namespace Jewelry_Store.RepositoryInterface
{
    public interface ICacheRepository
    {
        T GetCache<T>(string key);
        void SetCache<T>(string key, T value, int expireHours = CacheConstant.CacheExpireTimeInHours);
    }
}
