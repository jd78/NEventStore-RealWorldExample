NEventStore-RealWorldExample
============================

The code shows how to use NEventStore in the "real world". I implemented a little football match statistic.

The solution is composed by:

- Domain:
  The DDD model containing the Match

- Contracts: 
  The matches' mutables

- Events:
  The events that will be fired after we store a event set.

- Services:
  Cointains the match service (create, get, etc.)

- Infrastructure:
  The project's heart. Contains the Aggregate Factory, The Command Dispatcher, and the Event Handler.

NEventStore is configured in the IocBootstrapper file. It uses an in memory persistence. 
If you want to persist the data on a database to see how things are working behind the scene, 
uncomment the following lines

//.UsingSqlPersistence("FootballEventStore")
//.WithDialect(new MsSqlDialect())

and comment

.UsingInMemoryPersistence()

You need also to create a local database named FootballEventStore
