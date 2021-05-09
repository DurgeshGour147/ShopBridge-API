using Jewelry_Store.Common;
using Jewelry_Store.Domain;
using Jewelry_Store.DTO;
using Jewelry_Store.ProviderInterface;
using Jewelry_Store.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry_store.Provider
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IDBRepository _DBProvider;

        public AuthenticationProvider(IDBRepository dbRepository)
        {
            _DBProvider = dbRepository;
        }
        public async Task<Bearer> GenToken(AuthenticationDTORequest request)
        {
            if (!request.IsValid())
                return new Bearer { ErrorMessage = ErrorConstant.InValidRequest };

            UserDomain authenticateResponse = await Authenticate(request);
            if (authenticateResponse.IsNull())
                return new Bearer { HttpStatusCode = HttpStatusCode.Unauthorized };

            if (authenticateResponse.U_Password != request.Password)
                return new Bearer { ErrorMessage = ErrorConstant.InValidPassword };

            List<UserRoleDomain> userRoles = await Authorize(authenticateResponse.U_Id);
            if (!userRoles.HasRecords())
                return new Bearer { HttpStatusCode = HttpStatusCode.Forbidden };

            Dictionary<string, string> accessToken = new Dictionary<string, string>();
            accessToken.Add(AccessTokenKey.UserName, authenticateResponse.U_UserName);
            accessToken.Add(AccessTokenKey.Roles, userRoles.Select(x => x.UR_R_Id).Distinct().SerializeObject());
            accessToken.Add(AccessTokenKey.IssueTime, DateTime.Now.ToString());
            accessToken.Add(AccessTokenKey.ExpiryTime, DateTime.Now.AddHours(12).ToString());
            accessToken.Add(AccessTokenKey.AppName, "JewelleryStore");
            string token = CreateFinalAccessToken(accessToken);
            if (!string.IsNullOrEmpty(token))
                return new Bearer() { IsSuccess = true, HttpStatusCode = HttpStatusCode.OK, AccessToken = token };

            return new Bearer { ErrorMessage = ErrorConstant.AuthenticationDenied };
        }

        public async Task<UserDetailResponseDTO> GetUserDetail(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return new UserDetailResponseDTO() { ErrorMessage = ErrorConstant.InValidRequest };

            Dictionary<string, string> resp = accessToken.DecryptAccessTokenAES();
            if (!resp.HasRecords())
                return new UserDetailResponseDTO() { ErrorMessage = ErrorConstant.InValidToken };

            if (resp.ContainsKey(AccessTokenKey.ExpiryTime) && DateTime.Parse(resp[AccessTokenKey.ExpiryTime]) >= DateTime.Now)
            {
                if (resp.ContainsKey(AccessTokenKey.UserName))
                {
                    Expression<Func<UserDomain, bool>> expression = x => x.U_IsActive;
                    expression = expression.ExpressionAnd(x => x.U_UserName == resp[AccessTokenKey.UserName]);
                    UserDomain userDomainResponse = await _DBProvider.GetUserDetail(expression);
                    if (userDomainResponse.IsNotNull())
                    {
                        Expression<Func<UserRoleDomain, bool>> roleExpression = x => x.UR_IsActive;
                        roleExpression = roleExpression.ExpressionAnd(x => x.UR_U_Id == userDomainResponse.U_Id);
                        List<UserRoleDomain> userRoles = await _DBProvider.GetUserRoles(roleExpression);
                        if (userRoles.HasRecords())
                        {
                            List<RoleDomain> roles = await _DBProvider.GetRole();
                            List<RoleForBearerDTO> roleForBearerDTOs = new List<RoleForBearerDTO>();
                            userRoles.ForEach(x =>
                            {
                                roleForBearerDTOs.Add(new RoleForBearerDTO()
                                {
                                    RoleId = x.UR_R_Id,
                                    RoleName = roles.Where(y => y.R_Id == x.UR_R_Id).FirstOrDefault().R_Name
                                });
                            });

                            return new UserDetailResponseDTO
                            {
                                IsSuccess = true,
                                Roles = roleForBearerDTOs,
                                UserId = userDomainResponse.U_Id,
                                Username = userDomainResponse.U_UserName
                            };
                        }
                    }
                    else
                        return new UserDetailResponseDTO() { ErrorMessage = ErrorConstant.NoDataFound };
                }
            }
            else
                return new UserDetailResponseDTO() { ErrorMessage = ErrorConstant.ExpiryToken };

            return null;
        }

        private async Task<UserDomain> Authenticate(AuthenticationDTORequest request)
        {
            Expression<Func<UserDomain, bool>> expression = x => x.U_IsActive;
            expression = expression.ExpressionAnd(x => x.U_UserName == request.UserName);
            UserDomain response = await _DBProvider.GetUserDetail(expression);
            if (response.IsNotNull())
                return response;

            return null;
        }

        private async Task<List<UserRoleDomain>> Authorize(int userId)
        {
            Expression<Func<UserRoleDomain, bool>> expression = x => x.UR_IsActive;
            expression = expression.ExpressionAnd(x => x.UR_U_Id == userId);
            List<UserRoleDomain> response = await _DBProvider.GetUserRoles(expression);
            if (response.HasRecords())
                return response;

            return null;
        }

        private string CreateFinalAccessToken(Dictionary<string, string> accessToken)
        {
            if (!accessToken.HasRecords())
                return string.Empty;
            return accessToken.SerializeObject().Encrypt();
        }
    }
}
