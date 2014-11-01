using System;
using System.Collections;
using System.Collections.Generic;
using Autofac;
using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Infrastructure
{
    public class CommitDispatcher : ICommitDispatcher
    {
        public void Dispatch(IEvent commit)
        {
            Console.WriteLine("Object type {0} version: {1} dispatched", commit.GetType(), commit.Version);

            var eventHandlerEnumerableType = CreateEventHandlerEnumerableType(commit);
            //TODO fix service locator antipattern
            var handlers = (IEnumerable)IocBootstrapper.Container.Resolve(eventHandlerEnumerableType);
            foreach (var handler in handlers)
            {
                (handler as dynamic).Handle(commit as dynamic);
            }
        }

        private static Type CreateEventHandlerEnumerableType<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var genericEnumerableType = typeof(IEnumerable<>);
            var genericHandlerType = typeof(IEventHandler<>);
            var eventHandlerType = genericHandlerType.MakeGenericType(@event.GetType());
            var eventHandlerEnumerableType = genericEnumerableType.MakeGenericType(eventHandlerType);
            return eventHandlerEnumerableType;
        }
    }
}