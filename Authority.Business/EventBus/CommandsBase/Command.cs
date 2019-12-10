using Authority.Business.EventBus.IMessageBus;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Business.EventBus.CommandsBase
{
    public abstract class Command:Message
    {
        public DateTime Timestamp { get; private set; }
        //验证结果，需要引用FluentValidation
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        //定义抽象方法，是否有效
        public abstract bool IsValid();


    }
}
