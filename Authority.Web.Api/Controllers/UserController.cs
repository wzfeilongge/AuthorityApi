using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Model.Model;
using Authority.Web.Api.ControllerModel;
using Authority.Web.Api.JwtHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authority.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region  初始化接口 DI
        private readonly IUserServices _userServices;

        private readonly ILogger<UserController> _Apiloger;

        private readonly IJwtInterface _IJwtInterface;

        public UserController(IUserServices userServices, ILogger<UserController> Apiloger, IJwtInterface IJwtInterface)
        {
            _userServices = userServices;
            _Apiloger = Apiloger;
            _IJwtInterface = IJwtInterface;
        }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Login", Name = ("Login"))]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            var result = await _userServices.Login(request.UserName, request.PassWord);
            _Apiloger.LogInformation($"{HttpContext.Response.StatusCode}");
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
                    result.PassWord = "******";
                    result.AdminPassword = "******";
                }
                return Ok(new SucessModelData<User>(result));
            }
            return Ok(new JsonFailCatch("登录失败"));
        }

 

        /// <summary>
        /// 修改自己的状态
        /// </summary>
        /// <param name="User"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPut("ChangeStateForUp", Name = ("ChangeStateForUp"))]
        [Authorize(Policy = ("All"))]
        public async Task<IActionResult> ChangeStateForUp([FromBody] User User)
        {
            if (User != null)
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
        /// 修改自己的登录密码
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        [HttpPost("ChangePassWord", Name = ("ChangePassWord"))]
        [Authorize(Policy = ("All"))]
        public async Task<IActionResult> ChangePassWord([FromBody] ChangePassWordModel Model)
        {
            if (User != null)
            {
                var result = await _userServices.ChangePassword(Model.UserName,Model.OldPassword,Model.NewPassWord);
                if (result)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("修改用户失败"));
        }

  
    }
}