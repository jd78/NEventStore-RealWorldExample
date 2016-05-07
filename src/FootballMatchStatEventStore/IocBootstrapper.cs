using Autofac;
using Autofac.Builder;
using CommonDomain;
using CommonDomain.Core;
using CommonDomain.Persistence;
using FootballMatchStatEventStore.Contracts;
using FootballMatchStatEventStore.Events;
using FootballMatchStatEventStore.Infrastructure;
using FootballMatchStatEventStore.Services;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using EventStoreRepository = FootballMatchStatEventStore.Services.EventStoreRepository;

namespace FootballMatchStatEventStore
{
    public static class IocBootstrapper
    {
        public static IContainer Container { get; private set; }
        public static ContainerBuilder Builder;

        static IocBootstrapper()
        {
            Builder = new ContainerBuilder();
            Builder.Register(CreateNEventStore)
                .As<IStoreEvents>().SingleInstance();
            Builder.RegisterType<EventStoreRepository>()
                .As<IRepository>();
            Builder.RegisterType<ConflictDetector>()
                .As<IDetectConflicts>()
                .SingleInstance();
            Builder.RegisterType<AggregateFactory>()
                .As<IConstructAggregates>()
                .SingleInstance();
            Builder.RegisterType<CommitDispatchingPipelineHook>()
                .As<IPipelineHook>()
                .SingleInstance();
            Builder.RegisterType<CommitDispatcher>().As<ICommitDispatcher>().SingleInstance();

            Builder.RegisterType<MatchService>().As<IMatchService>().SingleInstance();

            Builder.RegisterEventHandler<MatchDeclaredEventHandler, MatchDeclared>();
            Builder.RegisterEventHandler<MatchStatusUpdatedEventHandler, MatchStatusUpdated>();
            
            Container = Builder.Build();
        }

        private static IStoreEvents CreateNEventStore(IComponentContext ctx)
        {
            var hook = ctx.Resolve<IPipelineHook>();
            var store = Wireup.Init()
                              .HookIntoPipelineUsing(hook)
                              .UsingSqlPersistence("FootballEventStore")
                              .WithDialect(new MsSqlDialect())
                              //.UsingInMemoryPersistence()
                              .InitializeStorageEngine()
                              .UsingJsonSerialization()
                              .Build();

            return store;
        }

        public static IRegistrationBuilder<THandler, ConcreteReflectionActivatorData, SingleRegistrationStyle>
            RegisterEventHandler<THandler, TEvent>(this ContainerBuilder builder)
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            return builder.RegisterType<THandler>()
                          .As<IEventHandler<TEvent>>()
                          .As<IEventHandler<TEvent>>();
        }
    }
}
