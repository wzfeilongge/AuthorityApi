using Authority.Applicaion.ViewModel;
using Authority.Model.BankModel;
using Authority.Model.Model;
using Authority.Model.Model.BankModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Business.Business
{
  public  interface IAuthorityBusinessInterface
    {
        UserViewModel GetDtoModel(User user);

        List<UserViewModel> GetDtoModels(List<User> user);

        DepartmentsViewModel GetDtoModelDepartment(Departments departments);

        List<DepartmentsViewModel> GetDtoModelDepartment(List<Departments> departments);


        CreateViewModel GetCreateDto(BusinessMan BusinessMan);

        List<CreateViewModel> GetCreateDtoList(List<BusinessMan> BusinessManList);


        CounterCuteGirl GetViewModelChangeCounterCuteGirl(CallNumberViewModel viewModel);




    }
}
