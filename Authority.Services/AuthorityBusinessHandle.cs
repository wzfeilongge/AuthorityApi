using System;
using System.Collections.Generic;
using System.Text;
using Authority.Applicaion.ViewModel;
using Authority.Model.BankModel;
using Authority.Model.Model;
using Authority.Model.Model.BankModel;
using AutoMapper;

namespace Authority.Business.Business
{
    public class AuthorityBusinessHandle : IAuthorityBusinessInterface
    {

        private IMapper _mapper { get; set; }
        public AuthorityBusinessHandle(IMapper mapper)
        {
            _mapper = mapper;
        }
        public UserViewModel GetDtoModel(User user)
        {
            return _mapper.Map<User,UserViewModel>(user);
        }

        public List<UserViewModel> GetDtoModels(List<User> user)
        {

            return _mapper.Map<List<User>, List<UserViewModel>>(user);
        }

        public DepartmentsViewModel GetDtoModelDepartment(Departments departments)
        {
            return _mapper.Map<Departments,DepartmentsViewModel>(departments);
        }

        public List<DepartmentsViewModel> GetDtoModelDepartment(List<Departments> departments)
        {
            return _mapper.Map<List<Departments>, List<DepartmentsViewModel>>(departments);
        }

        public CreateViewModel GetCreateDto(BusinessMan BusinessMan)
        {
            return _mapper.Map<BusinessMan,CreateViewModel>(BusinessMan);
        }

        public List<CreateViewModel> GetCreateDtoList(List<BusinessMan> BusinessManList)
        {
            return _mapper.Map<List<BusinessMan>,List<CreateViewModel>>(BusinessManList);
        }

        public CounterCuteGirl GetViewModelChangeCounterCuteGirl(CallNumberViewModel viewModel)
        {
            return _mapper.Map<CallNumberViewModel,CounterCuteGirl>(viewModel);
        }
    }
}
