using System;
using System.Reflection;
using CommonDomain;
using CommonDomain.Persistence;

namespace FootballMatchStatEventStore.Infrastructure
{
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento memento)
        {
            if (memento == null)
            {
                var constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Guid) }, null);
                return constructor.Invoke(new object[] {id}) as IAggregate;
            }

            var constructorMemento = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(IMemento) }, null);
            return constructorMemento.Invoke(new object[] { memento }) as IAggregate;
        }
    }
}

