using Authoritiy.IServices.BaseServices;
using Authority.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authoritiy.IServices
{
    public interface IDepartmentService : IBaseEntity<Departments>
    {
        Task<int> AddDepartment(Departments departments);

        Task<int> DelDepartment(Departments departments);

        Task<Departments> EditDepartment(Departments departments);

        Task<List<Departments>> QueryList();

        Task<Departments> QueryDepartment(string DepartmentName);
      
        Task<int> UpdateDepartments(Departments departments=null);

    }
}
