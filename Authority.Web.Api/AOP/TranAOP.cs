using Authority.Common.HttpHelper;
using Authority.IRepository.IUnitOfWord;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Authority.Web.Api.AOP.Filter
{
    public class TranAOP : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public TranAOP(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(UseTranAttribute)) is UseTranAttribute)
            {
                try
                {
                    Console.WriteLine($"UnintOfWord strat");
                    //_unitOfWork.BeginTran();
                    invocation.Proceed();
                    // 异步获取异常，普通的 try catch 外层不能达到目的，毕竟是异步的
                    if (IsAsyncMethod(invocation.Method))
                    {
                        if (invocation.Method.ReturnType == typeof(Task))
                        {
                            invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                                (Task)invocation.ReturnValue,
                                async () => await TestActionAsync(invocation),
                                ex =>
                                {
                                    _unitOfWork.Rollback();//事务回滚
                                });
                        }
                        else //Task<TResult>
                        {
                            invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                             invocation.Method.ReturnType.GenericTypeArguments[0],
                             invocation.ReturnValue,
                             async () => await TestActionAsync(invocation),
                             ex =>
                             {
                                 _unitOfWork.Rollback();//事务回滚
                             });
                        }
                    }
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    Console.WriteLine($"Rollback Transaction");
                    _unitOfWork.Rollback();
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }

        public static bool IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) || (method.ReturnType.IsGenericType &&
                method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

        private async Task TestActionAsync(IInvocation invocation)
        {
           
        }

     

        internal static class InternalAsyncHelper
        {
            public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
            {
                Exception exception = null;

                try
                {
                    await actualReturnValue;
                    await postAction();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    finalAction(exception);
                }
            }

            public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
            {
                Exception exception = null;

                try
                {
                    var result = await actualReturnValue;
                    await postAction();
                    return result;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    throw;
                }
                finally
                {
                    finalAction(exception);
                }
            }

            public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<Exception> finalAction)
            {
                return typeof(InternalAsyncHelper)
                    .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(taskReturnType)
                    .Invoke(null, new object[] { actualReturnValue, action, finalAction });
            }
        }
    }
}
