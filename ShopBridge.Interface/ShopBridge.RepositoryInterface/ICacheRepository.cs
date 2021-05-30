using Shop_Bridge.Common;
using System;

namespace Shop_Bridge.RepositoryInterface
{
    public interface ICacheRepository
    {
        T GetCache<T>(string key);
        void SetCache<T>(string key, T value, int expireHours = CacheConstant.CacheExpireTimeInHours);
    }
}
