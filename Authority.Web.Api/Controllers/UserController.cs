using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Applicaion.ViewModel;
using Authority.Business.Business;
using Authority.Common.Helper;
using Authority.Common.HttpHelper;
using Authority.Model.Model;
using Authority.Web.Api.ControllerModel;
using Authority.Web.Api.JwtHelper;
using Authority.Web.Api.PolicyRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Renci.SshNet.Messages.Authentication;
using StackExchange.Redis;

namespace Authority.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Permissions.Name)]
    public class UserController : ControllerBase
    {
        #region  初始化接口 DI
        private readonly IUserServices _userServices;

        private readonly IJwtInterface _IJwtInterface;

        private readonly IAuthorityBusinessInterface _authorityBusinessInterface;


        public UserController(IUserServices userServices, IJwtInterface IJwtInterface, IAuthorityBusinessInterface authorityBusinessInterface)
        {
            _userServices = userServices;
            _IJwtInterface = IJwtInterface;
            _authorityBusinessInterface = authorityBusinessInterface;
            // _unitOfWork = unitOfWork;
        }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Login", Name = ("Login"))]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.Login(request.UserName, request.PassWord);
                if (result != null)
                {
                    TokenModelJwt t = new TokenModelJwt
                    {
                        Role = result.Role,
                        Uid = result.Id,
                        Name = result.UserName
                    };
                    string token = _IJwtInterface.IssueJwt(t);
                    if (token != null)
                    {
                        result.Token = token;
                    }                  
                    var model = _authorityBusinessInterface.GetDtoModel(result);
                    return Ok(new SucessModelData<UserViewModel>(model));
                }
            }
            return Ok(new JsonFailCatch("登录失败"));
        }

        /// <summary>
        /// 修改自己的状态
        /// </summary>
        /// <param name="User"></param>     
        /// <returns></returns>
        [HttpPut("ChangeStateForUp", Name = ("ChangeStateForUp"))]
        public async Task<IActionResult> ChangeStateForUp([FromBody] User User)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ChangeUserState(User);
                if (result)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("修改用户失败"));
        }

        /// <summary>
        /// 修改自己的密码
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPut("ChangePassWord", Name = ("ChangePassWord"))]
        public async Task<IActionResult> ChangePassWord([FromBody] ChangePassWordModel Model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ChangePassword(Model.UserName, Model.OldPassword, Model.NewPassWord);
                if (result)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("修改用户失败"));
        }

        [HttpGet("Test", Name = "Test")]
        public async Task<IActionResult> Test()
        {
            var model = await _userServices.Test();
            return Ok(new SucessModelData<List<SnowShowModel>>(model));
        }

        [HttpPost("RegisterUser", Name = ("RegisterUser"))]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterModel RegisterModel)
        {
            if (ModelState.IsValid)
            {
                RegisterModel.UserPassword = FacePayEncrypt.Encrypt(RegisterModel.UserPassword);
                RegisterModel.OtherPassword = FacePayEncrypt.Encrypt(RegisterModel.OtherPassword);
                if (!(RegisterModel.UserPassword.Equals(RegisterModel.OtherPassword)))
                {
                    return Ok(new JsonFailCatch("两次密码输入不一致"));
                }
                var user = _authorityBusinessInterface.RegisterUserModel(RegisterModel);
                user.Id = SnowHelper.GetSnowId();
                user.Remarks = user.State == 1 ?"在职": "离职";
                var result = await _userServices.AddUser(user);
                if (result != null)
                {
                    return Ok(new SucessModel("注册成功"));
                }
            }
            return Ok(new JsonFailCatch("注册失败"));
        }

        [HttpGet("RefreshToekn", Name = ("RefreshToekn"))]
        public async Task<IActionResult> RefreshToekn([FromBody]RefreshModel TokenModel)
        {
            string jwtStr = string.Empty;
            if (string.IsNullOrEmpty(TokenModel.Token))
            {
                return Ok(new JsonFailCatch("Token 无效"));
            }
            var model = _IJwtInterface.SerializeJwt(TokenModel.Token);
            if (model != null)
            {
                var user = await _userServices.GetModelAsync(u => u.Id == model.Uid);
                if (user != null)
                {
                    model.Name = user.UserName;
                    jwtStr = _IJwtInterface.IssueJwt(model);                  
                    return Ok(new SucessModelData<object>(jwtStr));
                }
            }
            return Ok(new JsonFailCatch("Token 无效"));
        }
    }
}