using Authority.Applicaion.ViewModel;
using Authority.Model.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Applicaion.AutoMapper
{
   public class AutoMapperDto : Profile
    {

        public AutoMapperDto()
        {
            CreateMap<User, UserViewModel>();

            CreateMap<Departments,DepartmentsViewModel>();
           
        }



    }
}
