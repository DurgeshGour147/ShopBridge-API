using Shop_Bridge.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Bridge.ProviderInterface
{
   public interface IAuthenticationProvider
    {
        Task<Bearer> GenToken(AuthenticationDTORequest request);

        Task<UserDetailResponseDTO> GetUserDetail(string accessToken);

    }
}
