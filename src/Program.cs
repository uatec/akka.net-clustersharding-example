using System.IO;
using Akka.Actor;
using Akka.Cluster.Sharding;
using Akka.Configuration;
using System;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Config config = getConfig();

            ActorSystem system = ActorSystem.Create("mysystem", config);

            // register actor type as a sharded entity
            var region = ClusterSharding.Get(system).Start(
                typeName: "my-actor",
                entityProps: Props.Create<MyActor>(),
                settings: ClusterShardingSettings.Create(system),
                messageExtractor: new MessageExtractor());

            // send message to entity through shard region
            region.Tell(new Envelope(shardId: 1, entityId: 1, message: "hello"));

            while ( true ) 
            {
                Thread.Sleep(1000);
            }
        }

        private static Config getConfig()
        {
            Config config = File.ReadAllText("main.hocon");
 
            var isPoint = Environment.GetEnvironmentVariable("point") ?? "no";
            if (!isPoint.Equals("yes"))
            {
                config = ((Config)File.ReadAllText("follower.hocon")).WithFallback(config);
            }

            return config;
        }
    }
}
