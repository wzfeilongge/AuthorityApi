using System;
using System.Collections.Generic;
using System.Text;
using Authority.Common.Helper;
using StackExchange.Redis;

namespace Authority.Common.ReadisHelper.ReadisInterface
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly string _redisConnenctionString;
        public volatile ConnectionMultiplexer _redisConnection;
        private readonly object _redisConnectionLock = new object();



        public RedisCacheManager()
        {
            string redisConfiguration = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });//获取连接字符串
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            this._redisConnenctionString = redisConfiguration;
            this._redisConnection = GetRedisConnection();
        }

        public ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this._redisConnection != null && this._redisConnection.IsConnected)
            {
                return this._redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (_redisConnectionLock)
            {
                if (this._redisConnection != null)
                {
                    //释放redis连接
                    this._redisConnection.Dispose();
                }
                try
                {
                    this._redisConnection = ConnectionMultiplexer.Connect(_redisConnenctionString);
                }
                catch (Exception)
                {

                    throw new Exception("Redis服务未启用，请开启该服务");
                }
            }
            return this._redisConnection;
        }

        public void Clear()
        {
           foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    _redisConnection.GetDatabase().KeyDelete(key);
                }

            }
        }
        public TEntity Get<TEntity>(string key)
        {
            var value = _redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default;
            }

        }

        public bool Get(string key)
        {
            return _redisConnection.GetDatabase().KeyExists(key);
        }

        public string GetValue(string key)
        {
            return _redisConnection.GetDatabase().StringGet(key);
        }

        public void Remove(string key)
        {
            _redisConnection.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                _redisConnection.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        public bool SetValue(string key, byte[] value)
        {
            return _redisConnection.GetDatabase().StringSet(key, value, TimeSpan.FromSeconds(120));
        }

        public bool SetNx(string key, long time, double expireMS)
        {        
            return false;
        }
    }
}
