namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 10 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10> :
        EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9>, IEventHandler<TEvent10>
    {
        private readonly IEventStream<TEvent10> eventStream10;

        protected EventHandler(
            IEventStream<TEvent1> eventStream1, 
            IEventStream<TEvent2> eventStream2, 
            IEventStream<TEvent3> eventStream3, 
            IEventStream<TEvent4> eventStream4, 
            IEventStream<TEvent5> eventStream5, 
            IEventStream<TEvent6> eventStream6,
            IEventStream<TEvent7> eventStream7,
            IEventStream<TEvent8> eventStream8,
            IEventStream<TEvent9> eventStream9,
            IEventStream<TEvent10> eventStream10)
            : base(eventStream1, eventStream2, eventStream3, eventStream4, eventStream5, eventStream6, eventStream7, eventStream8, eventStream9)
        {
            this.eventStream10 = eventStream10;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream10.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent10 @event);
    }
}