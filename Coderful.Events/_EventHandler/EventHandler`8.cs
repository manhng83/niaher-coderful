namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 8 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8> :
        EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7>, IEventHandler<TEvent8>
    {
        private readonly IEventStream<TEvent8> eventStream8;

        protected EventHandler(
            IEventStream<TEvent1> eventStream1, 
            IEventStream<TEvent2> eventStream2, 
            IEventStream<TEvent3> eventStream3, 
            IEventStream<TEvent4> eventStream4, 
            IEventStream<TEvent5> eventStream5, 
            IEventStream<TEvent6> eventStream6,
            IEventStream<TEvent7> eventStream7,
            IEventStream<TEvent8> eventStream8)
            : base(eventStream1, eventStream2, eventStream3, eventStream4, eventStream5, eventStream6, eventStream7)
        {
            this.eventStream8 = eventStream8;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream8.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent8 @event);
    }
}