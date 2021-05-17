namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 3 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2, TEvent3> : EventHandler<TEvent1, TEvent2>, IEventHandler<TEvent3>
    {
        private readonly IEventStream<TEvent3> eventStream3;

        protected EventHandler(
            IEventStream<TEvent1> eventStream1,
            IEventStream<TEvent2> eventStream2,
            IEventStream<TEvent3> eventStream3)
            : base(eventStream1, eventStream2)
        {
            this.eventStream3 = eventStream3;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream3.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent3 @event);
    }
}
