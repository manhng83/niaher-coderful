namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 1 type of event.
    /// </summary>
    public abstract class EventHandler<TEvent> : EventHandler, IEventHandler<TEvent>
    {
        private readonly IEventStream<TEvent> eventStream;

        protected EventHandler(IEventStream<TEvent> eventStream)
        {
            this.eventStream = eventStream;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            this.RegisterSubscription(this.eventStream.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent @event);
    }
}
