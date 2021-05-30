using Shop_Bridge.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Bridge.RepositoryInterface
{
    public interface IDBRepository
    {
        Task<UserDomain> GetUserDetail(Expression<Func<UserDomain, bool>> predicate);
        Task<List<UserRoleDomain>> GetUserRoles(Expression<Func<UserRoleDomain, bool>> predicate);
        Task<List<RoleDomain>> GetRole();
        Task<bool> ActionOnIntentoryItem(List<InventoryItemDomain> request);
        Task<List<InventoryItemDomain>> GetIntentoryItem(Expression<Func<InventoryItemDomain, bool>> predicate, int pageNo, int pageSize);
        Task<bool> InActiveInventoryItem(List<int> id);
    }
}
