using Authoritiy.IServices;
using Authority.Common.Helper;
using Authority.Common.HttpHelper;
using Authority.IRepository;
using Authority.Model.Model;
using Authority.Services.BaseService;
using Microsoft.Extensions.Logging;
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
        private readonly IUserRepository _userServices;
        private readonly ILogger<UserServices> _myLogger;

        public UserServices(IUserRepository IUserRepository, ILogger<UserServices> myLogger)
        {
            base.BaseDal = IUserRepository;
            _userServices = IUserRepository;
            _myLogger = myLogger;
          
        }
        #endregion

        /// <summary>
        /// 加入一名员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> AddUser(User user)
        {
            user.PassWord = FacePayEncrypt.Encrypt(user.PassWord);
            var model = await _userServices.Add(user);
            if (model != null)
            {
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------增加一个员工 {user.UserName}");
                return model;
            }
            return null;
        }

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
            var model = await _userServices.GetModelAsync(u => u.UserName == UserName && u.PassWord == OldPassWord);
            if (model != null)
            {
                model.PassWord = FacePayEncrypt.Encrypt(NewPassWord);
              var istrue=  await _userServices.Modify(model);//修改属性
                if (istrue>1) {
                    _myLogger.LogInformation($"{DateTime.Now.ToString()}------{UserName}已经修改了密码");
                    return true;
                }
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{UserName}用户密码修改失败");
            }
            return false;
        }

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

        /// <summary>
        /// 删除一名员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(User user)
        {
            user.PassWord = FacePayEncrypt.Encrypt(user.PassWord);
            var model = await _userServices.DelBy(u => u.UserName == user.UserName);
            if (model > 0)
            {
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{user.UserName}员工已经被删除 ");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<User> Login(string name, string Password)
        {
            Password = FacePayEncrypt.Encrypt(Password);
            var model = await _userServices.GetModelAsync(u => u.UserName == name && u.PassWord == Password);
            if (model != null)
            {
                _myLogger.LogInformation($"{DateTime.Now.ToString()}------{model.UserName}登录成功 权限是{model.Role}");
                return (model);
            }
            return null;
        }

        /// <summary>
        /// 保存修改的Model
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public async Task<bool> Modfiy(User User)
        {
            int istrue = await _userServices.Modify(User);
            if (istrue > 0)
            {
                return true;
            }
            return false;
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
            var UserList = await _userServices.Query(u => u.Department == DepartmentName);
            return UserList;
        }


        /// <summary>
        /// 根据状态查询员工
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> QueryListInfornormal(int State)
        {
            var UserList = await QueryList();
            var list = UserList.Where(u => u.State == State).ToList();
            _myLogger.LogInformation($"正在查询状态为{State}的查询完毕共有{list.Count}条数据");
            return list;
        }

        
        public Task Test()
        {
            return Task.FromResult(1+1);
        }
    }
}
