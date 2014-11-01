using FootballMatchStatEventStore.Contracts;
using NEventStore;

namespace FootballMatchStatEventStore.Infrastructure
{
    public class CommitDispatchingPipelineHook : PipelineHookBase
    {
        private readonly ICommitDispatcher _commitDispatcher;

        public CommitDispatchingPipelineHook(ICommitDispatcher commitDispatcher)
        {
            _commitDispatcher = commitDispatcher;
        }

        public override void PostCommit(ICommit committed)
        {
            foreach (var evt in committed.Events)
            {
                _commitDispatcher.Dispatch(evt.Body as IEvent);
            }
        }
    }
}