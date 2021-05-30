using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shop_BridgeTests.Models
{
    public class BaseResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
    public class Bearer : BaseResponseDTO
    {
        public string AccessToken { get; set; }
    }

    public class AuthenticationDTORequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class UserDetailResponseDTO : BaseResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<RoleForBearerDTO> Roles { get; set; }
    }

    public class RoleForBearerDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UserDetailRequestDTO
    {
        public string AccessToken { get; set; }
    }
}
