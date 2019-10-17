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

        private readonly IAuthorityBusinessInterface _authorityBusinessInterface;

       // private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserServices userServices, ILogger<UserController> Apiloger, IJwtInterface IJwtInterface, IAuthorityBusinessInterface authorityBusinessInterface)
        {
            _userServices = userServices;
            _Apiloger = Apiloger;
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
                        result.PassWord = "******";
                        result.AdminPassword = "******";
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
        [Authorize(Policy = ("All"))]
        
        public async Task<IActionResult> ChangeStateForUp([FromBody] User User)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //_unitOfWork.BeginTran();
                    var result = await _userServices.ChangeUserState(User);
                   // _unitOfWork.Commit();
                    if (result)
                    {
                        return Ok(new SucessModel());
                    }
                }
                catch (Exception)
                {
                  //  _unitOfWork.Rollback();
                    var model = await _userServices.GetModelAsync(u => u == User);
                    if (model == null)
                    {
                        return Ok(new JsonFailCatch("出现异常，回滚成功"));
                    }
                    else
                    {
                        return Ok(new SucessModel());
                    }
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
        [Authorize(Policy = ("All"))]
       
        public async Task<IActionResult> ChangePassWord([FromBody] ChangePassWordModel Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                 //   _unitOfWork.BeginTran();
                    var result = await _userServices.ChangePassword(Model.UserName, Model.OldPassword, Model.NewPassWord);
                 //   _unitOfWork.Commit();
                    if (result)
                    {
                        return Ok(new SucessModel());
                    }
                }
                catch (Exception)
                {
                    //_unitOfWork.Rollback();
                    Model.NewPassWord = FacePayEncrypt.Encrypt(Model.NewPassWord);
                    var model = await _userServices.GetModelAsync(u => u.UserName == Model.UserName && u.PassWord == Model.NewPassWord);
                    if (model != null)
                    {
                        return Ok(new SucessModel());
                    }
                    else
                    {
                        return Ok(new JsonFailCatch("出现异常，修改用户密码失败"));
                    }
                }
            }
            return Ok(new JsonFailCatch("修改用户失败"));
        }

       
        [HttpGet("Test",Name ="Test")]
        
        public async Task<IActionResult> Test() {
            await _userServices.Test();
            return Ok(new SucessModel());
        
        
        }

    }
}