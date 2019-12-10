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
            CreateMap<User, UserViewModel>().ForMember(u => u.Token, view => view.MapFrom(viewmodel => viewmodel.Token))
                .ForMember(u => u.Role, view => view.MapFrom(viewmodel => viewmodel.Role))
                .ForMember(u => u.Department, view => view.MapFrom(viewmodel => viewmodel.Department))
                .ForMember(u => u.State, view => view.MapFrom(viewmodel => viewmodel.State))
                .ForMember(u => u.UserName, view => view.MapFrom(viewmodel => viewmodel.UserName))
                ;

            CreateMap<UserViewModel, User>();

            CreateMap<Departments, DepartmentsViewModel>();

            CreateMap<BusinessMan, CreateViewModel>();

            CreateMap<CallNumberViewModel, CounterCuteGirl>();


            CreateMap<UserRegisterModel, User>().ForMember(view => view.UserName, u => u.MapFrom(user => user.UserName))
                .ForMember(view=>view.PassWord,u=>u.MapFrom(user=>user.UserPassword));


        }
    }
}
