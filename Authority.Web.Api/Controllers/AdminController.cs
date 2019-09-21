﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Model.Model;
using Authority.Web.Api.ControllerModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authority.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        #region 初始化DI
        private readonly IUserServices _userServices;

        private readonly IDepartmentService _departmentService;



        private readonly ILogger<AdminController> _Apiloger;
        public AdminController(IUserServices userServices, ILogger<AdminController> Apiloger, IDepartmentService departmentService)
        {
            _userServices = userServices;
            _Apiloger = Apiloger;
            _departmentService = departmentService;
        }

        #endregion


        #region 管理员权限 员工类

        /// <summary>
        /// 加入一个员工
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        [HttpPut("AddUser", Name = ("AddUser"))]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> AddUser([FromBody] User User)
        {
            if (User != null)
            {
                var ThisUser = await _userServices.AddUser(User);
                if (ThisUser != null)
                {
                    var model = await _departmentService.QueryDepartment(ThisUser.Department);
                    if (model != null)
                    {
                        model.Count++;
                        await _departmentService.Modfiy(model);
                        _Apiloger.LogInformation($"成功添加一个员工{ThisUser.UserName}");
                        return Ok(new SucessModel());
                    }
                }
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
        [Authorize(Policy = ("SystemOrAdmin"))]
        public async Task<IActionResult> DeleteUser([FromBody] User User)
        {
            if (User != null)
            {
                var result = await _userServices.DeleteUser(User);
                if (result)
                {
                    _Apiloger.LogInformation($"成功删除一个员工{User.UserName}");
                    return Ok(new SucessModel());
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
        [Authorize(Policy = ("SystemOrAdmin"))]
        public async Task<IActionResult> ChangeState([FromBody] User Users)
        {
            if (Users != null)
            {
                var result = await _userServices.ChangeUserState(Users);
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
        [Authorize(Policy = ("SystemOrAdmin"))]
        public async Task<IActionResult> QueryUser([FromBody] int State)
        {
            var result = await _userServices.QueryListInfornormal(State);
            if (result != null)
            {
                return Ok(new SucessModelData<List<User>>(result));
            }
            return Ok(new JsonFailCatch("查询失败"));
        }

        #endregion



        #region 管理员权限 部门类

        /// <summary>
        /// 加入一个部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        [HttpPut("AddDepartment", Name = "AddDepartment")]
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> AddDepartment([FromBody]Departments departments)
        {
            if (departments != null)
            {
                var Cont = await _departmentService.AddDepartment(departments);
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
        [Authorize(Policy = "SystemOrAdmin")]
        public async Task<IActionResult> DelDepartment([FromBody] Departments departments)
        {
            if (departments != null)
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
        public async Task<IActionResult> EditDepartment([FromBody] Departments departments)
        {

            if (departments != null)
            {
                var model = await _departmentService.EditDepartment(departments);
                if (model != null)
                {
                    return Ok(new SucessModel());
                }
            }
            return Ok(new JsonFailCatch("编辑修改部门失败"));

        }



        #endregion




    }
}