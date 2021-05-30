using System;

namespace Shop_Bridge.DTO
{
    public class AuthenticationDTORequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(this.UserName))
                return false;

            if (string.IsNullOrEmpty(this.Password))
                return false;

            return true;
        }
    }

    public class Bearer : BaseResponseDTO
    {
        public string AccessToken { get; set; }
    }
}
