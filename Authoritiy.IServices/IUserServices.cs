using Authoritiy.IServices.BaseServices;
using Authority.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices
{
    public interface IUserServices : IBaseEntity<User>
    {

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        Task<User> Login(string name, string Password);

        /// <summary>
        /// 加入一名员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> AddUser(User user);

        /// <summary>
        /// 删除一个员工
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(User user);

        /// <summary>
        /// 修改员工的状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> ChangeUserState(User user);

        /// <summary>
        /// 查询所有的员工
        /// </summary>
        /// <returns></returns>
        Task<List<User>> QueryList();

        /// <summary>
        /// 根据状态查询员工
        /// </summary>
        /// <returns></returns>
        Task<List<User>> QueryListInfornormal(int State);

        /// <summary>
        /// 用户修改自己密码
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(string UserName, string OldPassWord, string NewPassWord);

        /// <summary>
        /// 部门名称查询当前部门所有的员工
        /// </summary>
        /// <param name="DepartmentName"></param>
        /// <returns></returns>
        Task<List<User>> QueryListInforDepartment(string DepartmentName);

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        Task Test();
    }
}
