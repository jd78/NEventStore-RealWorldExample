using FootballMatchStatEventStore.Contracts;

namespace FootballMatchStatEventStore.Infrastructure
{
    public interface ICommitDispatcher
    {
        void Dispatch(IEvent commit);
    }
}