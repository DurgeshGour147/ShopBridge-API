using System.Threading.Tasks;
using Jewelry_Store.Common;
using Jewelry_Store.DTO;
using Jewelry_Store.ProviderInterface;
using Microsoft.AspNetCore.Mvc;

namespace Jewelry_Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationProvider _authProvider;
        public AuthenticationController(IAuthenticationProvider authProvider)
        {
            _authProvider = authProvider;
        }

        [HttpPost("genToken")]
        public async Task<Bearer> GetGenToken(AuthenticationDTORequest request)
        {
            if (!request.IsValid())
                return new Bearer { ErrorMessage = ErrorConstant.InValidRequest };

            return await _authProvider.GenToken(request);
        }

        [HttpPost("userDetail")]
        public async Task<UserDetailResponseDTO> GetUserDetail(UserDetailRequestDTO request)
        {
            if (request.IsNull() || string.IsNullOrEmpty(request.AccessToken))
                return new UserDetailResponseDTO { ErrorMessage = ErrorConstant.InValidRequest };

            return await _authProvider.GetUserDetail(request.AccessToken);
        }
    }
}