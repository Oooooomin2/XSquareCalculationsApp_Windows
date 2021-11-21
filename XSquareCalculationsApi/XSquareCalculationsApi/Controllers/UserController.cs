using System.Net;
using System.Net.Http;
using System.Web.Http;
using XSquareCalculationsApi.Entities;
using XSquareCalculationsApi.Models;
using XSquareCalculationsApi.Repositories;

namespace XSquareCalculationsApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly IResolveUsersRepository _resolveUsersRepository;
        private readonly IPassword _password;

        public UserController(IResolveUsersRepository resolveUsersRepository, IPassword password)
        {
            _resolveUsersRepository = resolveUsersRepository;
            _password = password;
        }

        [HttpPost]
        public HttpResponseMessage Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var IsExistedUserName = _resolveUsersRepository.IsDuplicateUserRegist(user.UserName);
            if (IsExistedUserName)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse
                {
                    Content = "DuplicateUserName",
                    Message = "このユーザ名は既に使われています。"
                });
            }

            if (user.UserPassword.Length < 8)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse
                {
                    Content = "ShortPasswordLength",
                    Message = "パスワードは8文字以上にしてください。"
                });
            }

            user.PasswordSalt = _password.CreateSaltBase64();
            user.UserPassword = _password.CreatePasswordHashBase64(user.PasswordSalt, user.UserPassword);

            _resolveUsersRepository.CreateNewUser(user);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
