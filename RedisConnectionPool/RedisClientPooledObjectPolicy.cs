using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Utilities.RedisConnectionPool
{
    public class RedisClientPooledObjectPolicy : PooledObjectPolicy<ConnectionMultiplexer>
    {
        private readonly IOptions<RedisSettings> _redisSettings;

        public RedisClientPooledObjectPolicy(IOptions<RedisSettings> redisSettings)
        {
            _redisSettings = redisSettings;
        }

        public override ConnectionMultiplexer Create()
        {
            var config = new ConfigurationOptions();
            config.EndPoints.Add(_redisSettings.Value.HostName, _redisSettings.Value.Port);
            config.User = _redisSettings.Value.UserName;
            config.Password = _redisSettings.Value.Password;
            config.ConnectRetry = _redisSettings.Value.ConnectRetryCount;
            config.ReconnectRetryPolicy = new ExponentialRetry(1000, 10000);
            config.DefaultDatabase = _redisSettings.Value.Db;

            return ConnectionMultiplexer.Connect(config);
        }

        public override bool Return(ConnectionMultiplexer obj)
        {
            if (obj.IsConnected) return true;
            obj?.Close();
            obj?.Dispose();
            return false;
        }
    }
}