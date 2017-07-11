using Proto;

namespace PersistentMailbox
{
    public class ActorManager
    {
        private readonly IActorFactory _factory;

        public ActorManager(IActorFactory factory)
        {
            _factory = factory;
        }

        public PID Get<T>() where T: IActor
        {
            return _factory.GetActor<T>();
        }
    }
}