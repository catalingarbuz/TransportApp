namespace TransportApp.Infrastructure.Configurations
{
    /// <summary>
    /// Configuration for connecting to a Redis server, containing the connection string and a logical instance name.
    /// </summary>
    public class RedisConfiguration
    {
        public string ConnectionString { get; set; } = default!;
        public string InstanceName { get; set; } = default!;
    }
}
