using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Business.EventBus.IMessageBus
{
    public class Message : IRequest
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }

    }
}
