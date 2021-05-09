using Jewelry_Store.Common;
using Jewelry_Store.DTO;
using Jewelry_Store.ProviderInterface;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Jewelry_Store.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        private readonly List<int> _roleIds;
        private static IAuthenticationProvider _authProvider;

        public void Init(IServiceProvider serviceProvider)
        {
            _authProvider = serviceProvider.GetService<IAuthenticationProvider>();
        }
        public AuthorizationFilter(params int[] RoleIds)
        {
            if (RoleIds.HasRecords())
                _roleIds = RoleIds.ToList();

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                string AccessToken = null;

                if (context.ActionArguments.IsNotNull() && context.ActionArguments.First().Value is BaseRequestDTO)
                    AccessToken = (context.ActionArguments.First().Value as BaseRequestDTO).AccessToken;

                if (string.IsNullOrEmpty(AccessToken))
                    throw new UnauthorizedAccessException(ErrorConstant.AccessTokenNotFound);


                UserDetailResponseDTO userDetail = _authProvider.GetUserDetail(AccessToken).Result;
                if (userDetail.IsNull() || !userDetail.IsSuccess)
                    throw new UnauthorizedAccessException(ErrorConstant.InvalidAccessToken);

                if (_roleIds.HasRecords())
                {
                    if (!(userDetail.Roles.Any(x => _roleIds.Contains(x.RoleId))))
                        throw new UnauthorizedAccessException(ErrorConstant.PermissionDenied);
                }
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
        }

    }
}
