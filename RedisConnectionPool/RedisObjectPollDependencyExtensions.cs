using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace Utilities.RedisConnectionPool
{
    public static class RedisObjectPollDependencyExtensions
    {
        public static IServiceCollection AddRedisConnectionPool(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<RedisSettings>>();
                return new DefaultObjectPoolProvider() {MaximumRetained = settings.Value.MaxConnectionPool != 0 ? settings.Value.MaxConnectionPool : Environment.ProcessorCount * 2}
                    .Create(new RedisClientPooledObjectPolicy(settings));
            });

            return services;
        }
    }
}