using Authority.Applicaion.ViewModel;
using Authority.Model.BankModel;
using Authority.Model.Model;
using Authority.Model.Model.BankModel;
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

            CreateMap<BusinessMan, CreateViewModel>();

            CreateMap<CallNumberViewModel, CounterCuteGirl>();          
        }
    }
}
