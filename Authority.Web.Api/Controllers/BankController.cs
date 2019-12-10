using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authoritiy.IServices;
using Authority.Applicaion.ViewModel;
using Authority.Business;
using Authority.Business.Business;
using Authority.Web.Api.ControllerModel;
using Authority.Web.Api.PolicyRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authority.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Permissions.Name)]
    public class BankController : ControllerBase
    {
        private readonly IBankHandle _bankHandle;
        private readonly IBusinessManServices _businessManServices;
        private readonly ICounterCuteGirlServices _counterCuteGirlServices;
        private readonly IAuthorityBusinessInterface _authorityBusinessInterface;

        public BankController(IBankHandle bankHandle, IBusinessManServices businessManServices,
            ICounterCuteGirlServices counterCuteGirlServices,
            IAuthorityBusinessInterface authorityBusinessInterface)
        {
            _bankHandle = bankHandle;
            _businessManServices = businessManServices;
            _counterCuteGirlServices = counterCuteGirlServices;
            _authorityBusinessInterface = authorityBusinessInterface;
        }



        [HttpGet("CreateBusiness", Name = "CreateBusiness")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBusiness([FromBody] BankCreateModel bankCreateModel)
        {
            var query = await _bankHandle.GetBusinessMan();
            if (query.Count > 20)
            {
                return Ok(new JsonFailCatch("目前人员较多，请稍后在进行取号办理业务"));
            }
            //可以在写给接口 给前端进行控制 大于数量进行控制取号的数量
            if (ModelState.IsValid)
            {
                var model = await _businessManServices.Create(bankCreateModel.BankBusinessType);
                if (model != null)
                {
                    var models = _authorityBusinessInterface.GetCreateDto(model);
                    return Ok(new SucessModelData<CreateViewModel>(models));
                }
            }
            return Ok(new JsonFailCatch("办理失败"));
        }

        [HttpGet("CallNumber", Name = "CallNumber")]
        public async Task<IActionResult> CallNumber([FromBody]CallNumberViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _authorityBusinessInterface.GetViewModelChangeCounterCuteGirl(viewModel);
                var query = await _bankHandle.GetBusinessMan(); //得到当前都队列
                var id = query.Dequeue().Id; //先进先出
                var flag = await _counterCuteGirlServices.CallNumber(model, id);
                if (flag)
                {
                    return Ok(new SucessModelData<CallNumberViewModel>(viewModel));
                }
                else
                {
                    var result = await _counterCuteGirlServices.RepeatNumber(model, id);
                    if (result)
                    {
                        return Ok(new JsonResult("正在重新呼叫"));
                    }
                    else
                    {
                        var models = await _businessManServices.GetModelAsync(u => u.Id == id);
                        models.Istrue = true;
                        await _businessManServices.Modify(models);
                        return Ok(new JsonFailCatch("重呼失败,下一个用户"));
                    }
                }
            }
            return Ok(new JsonFailCatch("呼叫失败"));
        }
    }
}