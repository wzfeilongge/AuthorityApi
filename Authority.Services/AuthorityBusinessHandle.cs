using System.Collections.Generic;
using Authority.Applicaion.ViewModel;
using Authority.Model.BankModel;
using Authority.Model.Model;
using Authority.Model.Model.BankModel;
using AutoMapper;

namespace Authority.Business.Business
{
    public class AuthorityBusinessHandle : IAuthorityBusinessInterface
    {

        private IMapper Mapper { get; set; }
        public AuthorityBusinessHandle(IMapper mapper)
        {
            Mapper = mapper;
        }
        public UserViewModel GetDtoModel(User user)
        {
            return Mapper.Map<User, UserViewModel>(user);
        }

        public List<UserViewModel> GetDtoModels(List<User> user)
        {
            return Mapper.Map<List<User>, List<UserViewModel>>(user);
        }

        public DepartmentsViewModel GetDtoModelDepartment(Departments departments)
        {
            return Mapper.Map<Departments,DepartmentsViewModel>(departments);
        }

        public List<DepartmentsViewModel> GetDtoModelDepartment(List<Departments> departments)
        {
            return Mapper.Map<List<Departments>, List<DepartmentsViewModel>>(departments);
        }

        public CreateViewModel GetCreateDto(BusinessMan BusinessMan)
        {
            return Mapper.Map<BusinessMan,CreateViewModel>(BusinessMan);
        }

        public List<CreateViewModel> GetCreateDtoList(List<BusinessMan> BusinessManList)
        {
            return Mapper.Map<List<BusinessMan>,List<CreateViewModel>>(BusinessManList);
        }

        public CounterCuteGirl GetViewModelChangeCounterCuteGirl(CallNumberViewModel viewModel)
        {
            return Mapper.Map<CallNumberViewModel,CounterCuteGirl>(viewModel);
        }

        public User RegisterUserModel(UserRegisterModel registerModel)
        {
            return Mapper.Map<UserRegisterModel,User>(registerModel);
        }
    }
}
