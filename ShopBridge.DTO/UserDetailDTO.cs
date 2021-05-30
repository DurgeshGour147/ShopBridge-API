using System.Collections.Generic;

namespace Shop_Bridge.DTO
{

    public class UserDetailRequestDTO: BaseRequestDTO
    {
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
}
