using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Business.Business
{
   public  static class PublishMQ
    {
        public static void AddQueue(string QueueName, object obj)
        {
            //using (var channel = MqHelper.GetNewConnection().CreateModel())
            //{
            //    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            //    channel.BasicPublish(string.Empty, QueueName, null, bytes);
            //}
        }
    }
}
