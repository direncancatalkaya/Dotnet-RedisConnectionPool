namespace Utilities.RedisConnectionPool
{
    public class RedisSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int Db { get; set; }
        public int ConnectRetryCount { get; set; }
        public int MaxConnectionPool { get; set; }
    }
}