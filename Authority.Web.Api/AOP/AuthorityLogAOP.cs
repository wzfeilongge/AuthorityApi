using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Authority.Web.Api.AOP
{
    public class AuthorityLogAOP : IInterceptor
    {
        private readonly ILogger<AuthorityLogAOP> _Apiloger;

        public AuthorityLogAOP(ILogger<AuthorityLogAOP> Apiloger)
        {
            _Apiloger = Apiloger;
        }
        public void Intercept(IInvocation invocation)
        {
            var dataIntercept = $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")} " +
                $"当前执行方法：{ invocation.Method.Name} " +
                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";     
            //在被拦截的方法执行完毕后 继续执行当前方法
            invocation.Proceed();
            //  dataIntercept += ($"被拦截方法执行完毕，返回结果：{invocation.ReturnValue}");
            var type = invocation.Method.ReturnType;
            if (typeof(Task).IsAssignableFrom(type))
            {
                var resultProperty = type.GetProperty("Result");
                dataIntercept += ($"【执行完成结果】：{JsonConvert.SerializeObject(resultProperty.GetValue(invocation.ReturnValue))}");
            }
            else
            {
                dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
            }

            #region log4net
            _Apiloger.LogDebug(dataIntercept);
            #endregion
        }
    }
}
