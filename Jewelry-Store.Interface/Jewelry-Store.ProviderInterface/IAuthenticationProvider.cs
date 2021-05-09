using Jewelry_Store.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jewelry_Store.ProviderInterface
{
   public interface IAuthenticationProvider
    {
        Task<Bearer> GenToken(AuthenticationDTORequest request);

        Task<UserDetailResponseDTO> GetUserDetail(string accessToken);

    }
}
