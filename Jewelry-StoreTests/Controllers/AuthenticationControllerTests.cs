using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jewelry_StoreTests;
using Jewelry_Store.Common;
using Jewelry_StoreTests.Models;

namespace Jewelry_Store.Controllers.Tests
{
    [TestClass()]
    public class AuthenticationControllerTests
    {

        [TestMethod()]
        public void GetGenTokenTest()
        {
            Bearer bearer = TestUtils.GenToken();
            Assert.IsTrue(bearer.IsNotNull() && bearer.IsSuccess && !string.IsNullOrEmpty(bearer.AccessToken));
        }

        [TestMethod()]
        public void GetUserDetailTest()
        {
            UserDetailResponseDTO result = TestUtils.GetUserDetail();
            Assert.IsTrue(result.IsNotNull() && result.IsSuccess);
        }
    }


    public class TestUtils
    {
        public static Bearer GenToken()
        {
            AuthenticationDTORequest model = new AuthenticationDTORequest()
            {
                UserName = "regular@gmail.com",
                Password = "Welcome@321"
            };
            return RestServiceUtils.MakeRestCall<AuthenticationDTORequest, Bearer>(model, "Authentication/genToken", Constant.URL);
        }

        public static UserDetailResponseDTO GetUserDetail()
        {
            Bearer bearer = GenToken();
            if (bearer.IsNotNull() && bearer.IsSuccess)
            {
                UserDetailRequestDTO model = new UserDetailRequestDTO()
                {
                    AccessToken = bearer.AccessToken
                };
                return RestServiceUtils.MakeRestCall<UserDetailRequestDTO, UserDetailResponseDTO>(model, "Authentication/userDetail", Constant.URL);
            }
            return null;
        }
    }
}