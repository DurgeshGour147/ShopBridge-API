using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop_BridgeTests;
using Shop_BridgeTests.Models;

namespace Shop_Bridge.Controllers.Tests
{
    [TestClass()]
    public class AuthenticationControllerTests
    {

        [TestMethod()]
        public void GetGenTokenTest()
        {
            Bearer bearer = TestUtils.GenToken();
            Assert.IsTrue(bearer !=null && bearer.IsSuccess && !string.IsNullOrEmpty(bearer.AccessToken));
        }

        [TestMethod()]
        public void GetUserDetailTest()
        {
            UserDetailResponseDTO result = TestUtils.GetUserDetail();
            Assert.IsTrue(result != null && result.IsSuccess);
        }
    }


    public class TestUtils
    {
        public static Bearer GenToken()
        {
            AuthenticationDTORequest model = new AuthenticationDTORequest()
            {
                UserName = "ProductAdmin@gmail.com",
                Password = "Welcome@321"
            };
            return RestServiceUtils.MakeRestCall<AuthenticationDTORequest, Bearer>(model, "Authentication/genToken", Constant.URL);
        }

        public static UserDetailResponseDTO GetUserDetail()
        {
            Bearer bearer = GenToken();
            if (bearer != null && bearer.IsSuccess)
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
