using Authoritiy.IServices;
using Authority.Business.Business;
using Authority.Common.Helper;
using Authority.Common.HttpHelper;
using Authority.IRepository;
using Authority.Model.Model;
using Authority.Services.BaseService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class UserServices : BaseService<User>, IUserServices
    {
        #region  初始化 DI
        private readonly ILogger<UserServices> _myLogger;
        private readonly IUserRepository _userServices;
        private readonly IDepartmentRepository _departmentsRepository;

        public UserServices(IUserRepository IUserRepository, ILogger<UserServices> myLogger, IDepartmentRepository departmentsRepository)
        {
            base.BaseDal = IUserRepository;
            _userServices = IUserRepository;
            _myLogger = myLogger;
            _departmentsRepository = departmentsRepository;
        }
        #endregion

         [UseTran]
        /// <summary>
        /// 加入一名员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> AddUser(User user)
        {
            var flag = await _userServices.GetModelAsync(u=>u.UserName==user.UserName);
            if (flag!=null) {
                return null;
            }
            var UserModel = await _userServices.AddModel(user);
            if (UserModel > 0)
            {
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------增加一个员工 {user.UserName}");
                return (user);
            }
            return null;
        }

        [UseTran]
        /// <summary>
        /// 用户修改自己的密码
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(string UserName, string OldPassWord, string NewPassWord)
        {
            NewPassWord = FacePayEncrypt.Encrypt(NewPassWord);
            OldPassWord = FacePayEncrypt.Encrypt(OldPassWord);
            bool flag = false;
            var model = await _userServices.GetModelAsync(u => u.UserName == UserName && u.PassWord == OldPassWord);
            if (model != null)
            {
                model.PassWord = FacePayEncrypt.Encrypt(NewPassWord);
                var istrue = await _userServices.Modify(model);//修改属性
                if (istrue > 1)
                {
                    _myLogger.LogInformation($"{DateTime.Now.ToString()}------{UserName}已经修改了密码");
                    flag = true;
                }
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{UserName}用户密码修改失败");
            }
            return flag;
        }

        [UseTran]
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> ChangeUserState(User user)
        {
            user.PassWord = FacePayEncrypt.Encrypt(user.PassWord);
            var model = await _userServices.GetModelAsync(u => u.UserName == user.UserName && u.PassWord == user.PassWord);
            if (model != null)
            {
                model.State = user.State;
                model.Role = user.Role;
                model.StartTime = user.StartTime;
                model.EndTime = user.EndTime;
                await _userServices.Modify(model);//修改属性
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{user.UserName}已经修改了状态");
                return true;
            }
            return false;
        }

        [UseTran]
        /// <summary>
        /// 删除一名员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(User user)
        {
            user.PassWord = FacePayEncrypt.Encrypt(user.PassWord);
            bool flag = false;
            var model = await _userServices.DelBy(u => u.UserName == user.UserName);
            if (model > 0)
            {
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{user.UserName}员工已经被删除 ");
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<User> Login(string name, string Password)
        {
            User model = null;
            //     Password = FacePayEncrypt.Encrypt(Password);
            model = await _userServices.GetModelAsync(u => u.UserName == name && u.PassWord == Password);
            if (model != null)
            {
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{model.UserName}登录成功 权限是{model.Role}");
            }
            return model;
        }



        /// <summary>
        /// 查询所有员工
        /// </summary>
        /// <returns></returns>         
        public async Task<List<User>> QueryList()
        {
            var UserList = await _userServices.Query(u => u.Id > 0);
            return UserList;
        }

        /// <summary>
        /// 部门名称查询当前部门所有的员工
        /// </summary>
        /// <param name="DepartmentName"></param>
        /// <returns></returns>
        public async Task<List<User>> QueryListInforDepartment(string DepartmentName)
        {
            if (DepartmentName == string.Empty)
            {
                return null;
            }

            // var list = await _userServices.QueryMuch<Departments>((u)=>(u.Department==Departments);
            var UserList = await _userServices.Query(u => u.Department == DepartmentName);
            return UserList;
        }


        /// <summary>
        /// 根据状态查询员工
        /// </summary>
        /// <returns></returns>
        [Caching]
        public async Task<List<User>> QueryListInfornormal(int State)
        {
            var UserList = (await QueryList()).Where(u => u.State == State).ToList();
            _myLogger.LogInformation($"正在查询状态为{State}的查询完毕共有{UserList.Count}条数据");
            return UserList;
        }

        public async Task<User> QuerySelf(User user)
        {
            var model = await _userServices.GetModelAsync(u => u.Id == user.Id && u.UserName == user.UserName);
            if (model != null)
            {
                return model;
            }
            return null;
        }


        //[Caching]
        public async Task<List<SnowShowModel>> Test()
        {
            var list = await (from a in _departmentsRepository.GetAll(o => o.Id > 0)
                              join b in _userServices.GetAll(o => o.Id > 0) on a.DepartmentName equals b.Department
                              select new SnowShowModel
                              {
                                  One = a.Id,
                                  Two = b.Department
                              }).ToListAsync();
            return list;
        }
    }
}
