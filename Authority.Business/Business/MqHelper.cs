using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Authority.Business.Business
{
    public static class MqHelper
    {
        private static IConnection _connection;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        //public static IConnection GetConnection()
        //{
        //    if (_connection != null) return _connection;
        //    _connection = GetNewConnection();
        //    return _connection;
        //}

        //public static IConnection GetNewConnection()
        //{
        //    //从工厂中拿到实例 本地host、用户admin
        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = "localhost",
        //        UserName = "guest",
        //        Password = "guest",
        //    };
        //    _connection = factory.CreateConnection();
        //    return _connection;
        //}
    }
}
