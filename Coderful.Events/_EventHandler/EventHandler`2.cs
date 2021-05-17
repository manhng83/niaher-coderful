namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 2 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2> : EventHandler<TEvent1>, IEventHandler<TEvent2>
    {
        private readonly IEventStream<TEvent2> eventStream2;

        protected EventHandler(
            IEventStream<TEvent1> eventStream1, 
            IEventStream<TEvent2> eventStream2)
            : base(eventStream1)
        {
            this.eventStream2 = eventStream2;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream2.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent2 @event);
    }
}
