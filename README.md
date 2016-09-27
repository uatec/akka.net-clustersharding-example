# Akka.NET Cluster Sharding Example

This project is a worked example of the (Akka.Cluster.Sharding module documentation)[http://getakka.net/docs/clustering/cluster-sharding].

## Prerequisites

.NET Command Line Tools (1.0.0-preview2-003121)

## How To Run

```
dotnet restore
cd src
```

For single seed node:
```
seed=yes dotnet run
```

For unlimited follower node in a different console:
```
dotnet run
```

## Expected Behaviour

After the follower node connects to the seed, one of the nodes should start a 
single instance of `MyActor` which should print `Actor Instantiated` to the console 
once across the entire cluster (declaring a single instance of the actor is now
active in the cluster), followed by `Received type: String` (once for each connected node)
describing that a message was received buy the actor.

## The Problem!

After running both the `seed` and the `follower` the cluster fails to start an instance
of `MyActor` and logs a warning:

```
[WARNING][27/09/2016 14:13:20][Thread 0020][[akka://mysystem/user/sharding/my-actor#674959404]] Trying to register to coordinator at [], but no acknowledgement. Total [1] buffered messages.
```

Additionally, when the follower node connects to the cluster, the seed node will fail
to handle messages sent do it with the following error:

```
[ERROR][27/09/2016 14:13:19][Thread 0009][[akka://mysystem/system/endpointManager/reliableEndpointWriter-akka.tcp%3A%2F%2Fmysystem%40127.0.0.1%3A59088-1/endpointWriter#1562219685]] Dropping message [Akka.Actor.ActorSelectionMessage] for non-local recipient [[akka.tcp://birchd@127.0.0.1:4053/]] arriving at [akka.tcp://birchd@127.0.0.1:4053] inbound addresses [akka.tcp://mysystem@127.0.0.1:4053]
Cause: Unknown
```