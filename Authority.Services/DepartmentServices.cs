using Authoritiy.IServices;
using Authority.Common.HttpHelper;
using Authority.IRepository;
using Authority.Model.Model;
using Authority.Services.BaseService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Services
{
    public class DepartmentServices : BaseService<Departments>, IDepartmentService
    {
        #region Di 注入

        private readonly ILogger<Departments> _myLogger;

        private readonly IDepartmentRepository _DepartmentRepository;

        private readonly IUserRepository _UserRepository;

        public DepartmentServices(IDepartmentRepository DepartmentRepository, IUserRepository UserRepository, ILogger<Departments> myLogger)
        {
            _myLogger = myLogger;
            _DepartmentRepository = DepartmentRepository;
            _UserRepository = UserRepository;
            BaseDal = DepartmentRepository;
        }

        #endregion

        [UseTran]
        /// <summary>
        /// 加入一个部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public async Task<int> AddDepartment(Departments departments)
        {
            var AddModel = _DepartmentRepository.AddModel(departments); //数据库中加入一个部门的模型
            _myLogger.LogInformation($"成功加入{departments.DepartmentName}部门-------{DateTime.Now.ToString()}");
            return await AddModel;
        }
        [UseTran]
        /// <summary>
        /// 删除一个部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public async Task<int> DelDepartment(Departments departments)
        {
            var DelModel = _DepartmentRepository.DelBy(u => u.Id == departments.Id && u.DepartmentName == departments.DepartmentName); //数据库中删除一个部门的模型
            _myLogger.LogInformation($"成功删除{departments.DepartmentName}------{DateTime.Now.ToString()}");
            return await DelModel;
        }

        [UseTran]
        /// <summary>
        /// 编辑一个部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public async Task<Departments> EditDepartment(Departments departments)
        {
            var EditModel = await _DepartmentRepository.GetModelAsync(u => u.DepartmentName == departments.DepartmentName && u.Id == departments.Id);
            if (EditModel != null)
            {
                _myLogger.LogInformation($"正在编辑{departments.DepartmentName}的信息");
                EditModel.Count = departments.Count;
                await _DepartmentRepository.Modify(EditModel);
                return EditModel;
            }
            return null;
        }

        /// <summary>
        /// 查询单个部门
        /// </summary>
        /// <param name="DepartmentName"></param>
        /// <returns></returns>
        public async Task<Departments> QueryDepartment(string DepartmentName)
        {
            var model = await _DepartmentRepository.GetModelAsync(u => u.DepartmentName == DepartmentName);
            _myLogger.LogInformation($"正在查询{DepartmentName}部门------{DateTime.Now.ToString()}");
            if (model != null)
            {
                return model;
            }
            return null;
        }

        /// <summary>
        /// 查询所有的部门
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public async Task<List<Departments>> QueryList()
        {
            var model = await _DepartmentRepository.Query(u => u.DepartmentName != null);
            _myLogger.LogInformation($"正在查询所有的部门------{DateTime.Now.ToString()}");
            if (model != null)
            {
                return model;
            }
            return null;
        }

      
        /// <summary>
        /// 更新department数据库中的数据 速度较慢
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdateDepartments(Departments departments=null)
        {
            if (departments==null) {

                departments = new Departments();
            }
         
            return  await _DepartmentRepository.UpdateContext(departments);
        
        }    
    }
}
