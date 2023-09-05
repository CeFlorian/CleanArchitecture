using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Testcontainers.MongoDb;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace NorthWind.Shared
{
    public class ContainerFixture : IAsyncDisposable
    {

        readonly MongoDbContainer MongoDB;
        readonly RabbitMqContainer RabbitMQ;
        readonly MsSqlContainer SQLServer;
        readonly INetwork Network;

        public ContainerFixture()
        {

            //const string connectionString = $"server=SQLServerIntegrationTest;user id={MsSqlBuilder.DefaultUsername};password={MsSqlBuilder.DefaultPassword};database={MsSqlBuilder.DefaultDatabase}";

            Network = new NetworkBuilder()
              .Build();

            //MongoDB = new MongoDbContainer(new MongoDbConfiguration("root", "Ab12345678"), Logger);
            //RabbitMQ = new RabbitMqContainer(new RabbitMqConfiguration("gest", "gest"), Logger);

            RabbitMQ = new RabbitMqBuilder()
                .WithImage("rabbitmq:3.12.4-management")
                .WithName("RabbitMQIntegrationTest-" + Guid.NewGuid().ToString())
                .WithUsername("root")
                .WithPassword("Ab12345678")
                .WithPortBinding("15672", true)
                .WithPortBinding("5672", true)
                //.WithPortBinding("15673", "15672")
                //.WithPortBinding("5673", "5672")
                .WithNetwork(Network)
                .WithNetworkAliases("RabbitMQIntegrationTest")
                .Build();

            MongoDB = new MongoDbBuilder()
                .WithImage("mongo")
                .WithName("MongoDBIntegrationTest-" + Guid.NewGuid().ToString())
                .WithUsername("root")
                .WithPassword("Ab12345678")
                .WithPortBinding("27017", true)
                //.WithPortBinding("27018", "27017")
                .DependsOn(RabbitMQ)
                .WithNetwork(Network)
                .WithNetworkAliases("MongoDBIntegrationTest")
                .Build();

            // Contraseña corresponde al usuario SA
            SQLServer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithName("SQLServerIntegrationTest-" + Guid.NewGuid().ToString())
                .WithEnvironment("ACCEPT_EULA", "Y")
                .WithPassword("Ab12345678")
                //.WithEnvironment("SA_PASSWORD", "Ab12345678")
                //.WithEnvironment("MSSQL_SA_PASSWORD", "Ab12345678")
                .WithPortBinding("1433", true)
                //.WithPortBinding("1434", "1433")
                .WithNetwork(Network)
                .WithNetworkAliases("SQLServerIntegrationTest")
                .Build();

        }

        public Task MongoDBStart()
        {
            return MongoDB.StartAsync();
        }

        public Task RabbitMQStart()
        {
            return RabbitMQ.StartAsync();
        }

        public Task SQLServerStart()
        {
            return SQLServer.StartAsync();
        }

        public Task MongoDBStop()
        {
            return MongoDB.StopAsync();
        }

        public Task RabbitMQStop()
        {
            return RabbitMQ.StopAsync();
        }

        public Task SQLServerStop()
        {
            return SQLServer.StopAsync();
        }

        public ValueTask SQLServerDispose()
        {
            return SQLServer.DisposeAsync();
        }

        public ValueTask RabbitMQDispose()
        {
            return RabbitMQ.DisposeAsync();
        }

        public ValueTask MongoDBDispose()
        {
            return MongoDB.DisposeAsync();
        }

        public string GetMongoDBHostPort()
        {
            return MongoDB.GetMappedPublicPort("27017").ToString();
        }

        public string GetMongoDBHostname()
        {
            return MongoDB.Hostname;
        }

        public string GetSQLServerHostPort()
        {
            return SQLServer.GetMappedPublicPort("1433").ToString();
        }

        public string GetSQLServerHostname()
        {
            return SQLServer.Hostname;
        }
        public string GetRabbitMQHostPort()
        {
            return RabbitMQ.GetMappedPublicPort("5672").ToString();
        }

        public string GetRabbitMQHostname()
        {
            return RabbitMQ.Hostname;
        }

        public string GetMongoDBConnectionString()
        {
            return MongoDB.GetConnectionString();
        }

        public string GetSQLServerConnectionString()
        {
            return SQLServer.GetConnectionString();
        }

        public string GetRabbitMQConnectionString()
        {
            return RabbitMQ.GetConnectionString();
        }


        public Task InitializeAsync()
        {
            Task sql = SQLServer.StartAsync();
            Task rabbit = RabbitMQ.StartAsync();
            Task mongo = MongoDB.StartAsync();

            //await sql;
            //await rabbit;
            //await mongo;

            return Task.WhenAll(new Task[] { sql, rabbit, mongo });

        }

        public async ValueTask DisposeAsync()
        {
            ValueTask sql = SQLServer.DisposeAsync();
            ValueTask rabbit = RabbitMQ.DisposeAsync();
            ValueTask mongo = MongoDB.DisposeAsync();

            await sql;
            await rabbit;
            await mongo;

            await Network.DisposeAsync();

        }

    }
}
