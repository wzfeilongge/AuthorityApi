﻿using System;
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
using Authority.Web.Api.PolicyRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authority.Web.Api.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    
    public class AdminController : ControllerBase
    {

        #region 初始化DI
        private readonly IUserServices _userServices;

        private readonly IDepartmentService _departmentService;

        private readonly ILogger<AdminController> _Apiloger;

        private readonly IAuthorityBusinessInterface _authorityBusinessInterface;

        private const string RabbitMqName = "UserAdd";

        //private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUserServices userServices, ILogger<AdminController> Apiloger, IDepartmentService departmentService,
             IAuthorityBusinessInterface authorityBusinessInterface)
        {
            _userServices = userServices;
            _Apiloger = Apiloger;
            _departmentService = departmentService;
            _authorityBusinessInterface = authorityBusinessInterface;
            //_unitOfWork = unitOfWork;
        }

        #endregion

        #region 管理员权限 员工类
        /// <summary>
        /// 加入一个员工
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        [HttpPost("AddUser", Name = ("AddUser"))]     
        public async Task<IActionResult> AddUser([FromBody] User User)
        {
            if (ModelState.IsValid)
            {
                User.Id = Convert.ToInt32(SnowHelper.GetSnowId());
                PublishMQ.AddQueue(Business.Business.Enum.AddUser.ToString(), User);
                await _departmentService.UpdateDepartments();
            }
            _Apiloger.LogInformation($"请求增加一个员工，但是失败了");
            return Ok(new JsonFailCatch("用户新增失败"));
        }

        /// <summary>
        /// 删除一个员工
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser", Name = ("DeleteUser"))]       
        public async Task<IActionResult> DeleteUser([FromBody] User User)
        {
            if (ModelState.IsValid)
            {
                //_unitOfWork.BeginTran();
                var result = await _userServices.DeleteUser(User);
                //_unitOfWork.Commit();
                if (result)
                {
                    var model = await _departmentService.QueryDepartment(User.Department);
                    if (model != null)
                    {
                        model.Count--;
                        await _departmentService.Modfiy(model);
                        _Apiloger.LogInformation($"成功删除一个员工{nameof(User.UserName)}");
                        return Ok(new SucessModel());
                    }
                }
            }
            return Ok(new JsonFailCatch("用户删除失败"));
        }

        /// <summary>
        /// 高级权限修改他人的状态
        /// </summary>
        /// <param name="Users"></param>
        /// <returns></returns>
        [HttpPut("ChangeState", Name = ("ChangeState"))]
        public async Task<IActionResult> ChangeState([FromBody] User Users)
        {
            if (ModelState.IsValid)
            {
                //  _unitOfWork.BeginTran();
                var result = await _userServices.ChangeUserState(Users);
                //_unitOfWork.Commit();
                if (result)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("修改用户失败"));
        }


        /// <summary>
        /// 根据状态查询所有的员工
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpGet("QueryUser", Name = ("QueryUser"))]
        [Authorize(Permissions.Name)]
        public async Task<IActionResult> QueryUser(int State)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.QueryListInfornormal(State);
                if (result != null)
                {
                    if (result.Count == 0)
                    {
                        return Ok(new SucessModelData<object>(null));
                    }

                    var ViewModel = _authorityBusinessInterface.GetDtoModels(result);
                    return Ok(new SucessModelData<List<UserViewModel>>(ViewModel));
                }
            }
            return Ok(new JsonFailCatch("查询失败"));
        }

        /// <summary>
        /// 修改员工部门
        /// </summary>
        /// <param name="ChangeModel"></param>
        /// <returns></returns>
        [HttpPut("ChangeUserDepartment", Name = ("ChangeUserDepartment"))]
        public async Task<IActionResult> ChangeUserDepartment([FromBody] ChangeDepartmentModel ChangeModel)
        {
            if (ModelState.IsValid)
            {


                var model = await _userServices.GetModelAsync(u => u.UserName == ChangeModel.UserName && u.Department == ChangeModel.OldDepartment);

                if (model != null)
                {
                    model.Department = ChangeModel.NewDepartment;
                    var istruenew = await _userServices.Modfiy(model);
                    if (istruenew > 0)
                    {
                        return Ok(new SucessModel());
                    }
                }
            }
            return Ok(new JsonFailCatch("编辑失败"));
        }

        /// <summary>
        /// 根据部门名称查询当前部门所有的员工
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInforDepartment", Name = "GetUserInforDepartment")]
        public async Task<IActionResult> GetUserInforDepartment([FromBody] string DepartmentName)
        {
            if (ModelState.IsValid)
            {
                var UserList = await _userServices.QueryListInforDepartment(DepartmentName);
                if (UserList != null)
                {
                    var viewModel = _authorityBusinessInterface.GetDtoModels(UserList);
                    return Ok(new SucessModelData<List<UserViewModel>>(viewModel));
                }
            }
            return Ok(new SucessModelData<object>(null));

        }
        #endregion

        #region 管理员权限 部门类

        /// <summary>
        /// 加入一个部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        [HttpPost("AddDepartment", Name = "AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody]Departments departments)
        {
            if (ModelState.IsValid)
            {

                // _unitOfWork.BeginTran();
                var Cont = await _departmentService.AddDepartment(departments);
                //_unitOfWork.Commit();
                if (Cont > 0)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("加入部门失败"));
        }

        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        [HttpDelete("DelDepartment", Name = "DelDepartment")]
        public async Task<IActionResult> DelDepartment([FromBody] Departments departments)
        {
            if (ModelState.IsValid)
            {
                var Cont = await _departmentService.DelDepartment(departments);

                if (Cont > 0)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("删除部门失败"));
        }

        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        [HttpPut("EditDepartment", Name = "EditDepartment")]
        public async Task<IActionResult> EditDepartment([FromBody] Departments departments)
        {
            if (ModelState.IsValid)
            {
                var model = await _departmentService.EditDepartment(departments);
                if (model != null)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("编辑修改部门失败"));
        }

        /// <summary>
        /// 查询所有的部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryListDepartment", Name = "QueryListDepartment")]
        public async Task<IActionResult> QueryListDepartment()
        {
            var model = await _departmentService.QueryList();
            if (model != null)
            {
                var ViewModel = _authorityBusinessInterface.GetDtoModelDepartment(model);
                return Ok(new SucessModelData<List<DepartmentsViewModel>>(ViewModel));
            }
            return Ok(new SucessModelData<object>(null));
        }


        /// <summary>
        /// 更新数据库中的数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdateDepartment", Name = "UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment()
        {
            var count = await _departmentService.UpdateDepartments();

            return Ok(new SucessModelCount(count));
        }
        #endregion
    }
}