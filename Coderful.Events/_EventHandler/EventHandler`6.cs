namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 6 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6> :
        EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5>, IEventHandler<TEvent6>
    {
        private readonly IEventStream<TEvent6> eventStream6;

        protected EventHandler(
            IEventStream<TEvent1> eventStream1, 
            IEventStream<TEvent2> eventStream2, 
            IEventStream<TEvent3> eventStream3, 
            IEventStream<TEvent4> eventStream4, 
            IEventStream<TEvent5> eventStream5, 
            IEventStream<TEvent6> eventStream6)
            : base(eventStream1, eventStream2, eventStream3, eventStream4, eventStream5)
        {
            this.eventStream6 = eventStream6;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream6.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent6 @event);
    }
}