namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 4 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2, TEvent3, TEvent4> : EventHandler<TEvent1, TEvent2, TEvent3>, IEventHandler<TEvent4>
    {
        private readonly IEventStream<TEvent4> eventStream4;

        protected EventHandler(
            IEventStream<TEvent1> eventStream1, 
            IEventStream<TEvent2> eventStream2, 
            IEventStream<TEvent3> eventStream3, 
            IEventStream<TEvent4> eventStream4)
            : base(eventStream1, eventStream2, eventStream3)
        {
            this.eventStream4 = eventStream4;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream4.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent4 @event);
    }
}