using Shop_Bridge.Common;
using Shop_Bridge.Common.DBConnection;
using Shop_Bridge.Domain;
using Shop_Bridge.RepositoryInterface;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shop_Bridge.DBRepository
{
    public class DBRepository : IDBRepository
    {
        private readonly ICacheRepository _cacheRepository;

        public DBRepository(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<List<RoleDomain>> GetRole()
        {
            List<RoleDomain> roles = _cacheRepository.GetCache<List<RoleDomain>>(CacheConstant.RoleTableCache);
            if (roles.HasRecords())
                return roles;

            using (var db = new ShopBridgeConnection())
            using (var trans = await db.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
            {
                roles = await db.GetTable<RoleDomain>()
                               .Where(x => x.R_IsActive)
                               .ToListAsync();
                _cacheRepository.SetCache<List<RoleDomain>>(CacheConstant.RoleTableCache, roles, CacheConstant.CacheExpireTimeInHours);
            }
            return roles;
        }

        public async Task<UserDomain> GetUserDetail(Expression<Func<UserDomain, bool>> predicate)
        {
            if (predicate.IsNull())
                return default;

            using (var db = new ShopBridgeConnection())
            using (var trans = await db.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
            {
                return await db.GetTable<UserDomain>()
                               .Where(predicate)
                               .FirstOrDefaultAsync();
            }
        }

        public async Task<List<UserRoleDomain>> GetUserRoles(Expression<Func<UserRoleDomain, bool>> predicate)
        {
            if (predicate.IsNull())
                return default;

            using (var db = new ShopBridgeConnection())
            using (var trans = await db.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
            {
                return await db.GetTable<UserRoleDomain>()
                               .Where(predicate)
                               .ToListAsync();
            }
        }

        public async Task<bool> ActionOnIntentoryItem(List<InventoryItemDomain> request)
        {
            if (!request.HasRecords())
                return false;

            using (var db = new ShopBridgeConnection())
            using (var trans = await db.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
            {
                request.ForEach(x =>
                {
                    if (x.I_Id > 0)
                    {

                        InventoryItemDomain itemDomain = db.GetTable<InventoryItemDomain>().Where(y => y.I_Id == x.I_Id).FirstOrDefault();
                        if (itemDomain.IsNotNull())
                        {
                            x.I_CreatedOn = itemDomain.I_CreatedOn;
                            x.I_ModifiedOn = DateTime.Now;
                            db.Update<InventoryItemDomain>(x);
                        }
                    }
                    else
                    {
                        x.I_CreatedOn = DateTime.Now;
                        x.I_ModifiedOn = DateTime.Now;
                        db.InsertWithInt32Identity<InventoryItemDomain>(x);
                    }
                });
                await trans.CommitAsync();
            }
            return true;
        }

        public async Task<List<InventoryItemDomain>> GetIntentoryItem(Expression<Func<InventoryItemDomain, bool>> predicate, int pageNo, int pageSize)
        {
            List<InventoryItemDomain> result = null;
            long totalRecords = default;
            using (var db = new ShopBridgeConnection())
            using (var trans = await db.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
            {

                if (predicate.IsNotNull())
                {
                    totalRecords = db.GetTable<InventoryItemDomain>()
                                    .Where(predicate)
                                    .Count();
                    if (totalRecords > 0)
                    {

                        result = await db.GetTable<InventoryItemDomain>()
                                       .Where(predicate)
                                       .Skip((pageNo - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
                    }
                }
                else
                    result = await db.GetTable<InventoryItemDomain>()
                                  .Skip((pageNo - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
            }
            return result;
        }

        public async Task<bool> InActiveInventoryItem(List<int> id)
        {
            if (!id.HasRecords())
                return false;

            using (var db = new ShopBridgeConnection())
            using (var trans = await db.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
            {
                db.GetTable<InventoryItemDomain>()
                  .Where(z => id.Contains(z.I_Id))
                  .Set(y => y.I_IsActive, false)
                  .Set(y => y.I_ModifiedOn, DateTime.Now)
                  .Update();

                await trans.CommitAsync();
            }
            return true;
        }
    }
}
