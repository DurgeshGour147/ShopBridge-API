using Jewelry_Store.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry_Store.RepositoryInterface
{
    public interface IDBRepository
    {
        Task<UserDomain> GetUserDetail(Expression<Func<UserDomain, bool>> predicate);
        Task<List<UserRoleDomain>> GetUserRoles(Expression<Func<UserRoleDomain, bool>> predicate);
        Task<List<RoleDomain>> GetRole();
    }
}
