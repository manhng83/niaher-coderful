namespace Coderful.Events
{
    /// <summary>
    /// This class implements IEventHandler interface which can handle 10 event types.
    /// </summary>
    public abstract class EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10, TEvent11, TEvent12, TEvent13, TEvent14, TEvent15> :
        EventHandler<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10, TEvent11, TEvent12, TEvent13, TEvent14>, IEventHandler<TEvent15>
    {
        private readonly IEventStream<TEvent15> eventStream15;

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
            IEventStream<TEvent10> eventStream10,
            IEventStream<TEvent11> eventStream11,
            IEventStream<TEvent12> eventStream12,
            IEventStream<TEvent13> eventStream13,
            IEventStream<TEvent14> eventStream14,
            IEventStream<TEvent15> eventStream15)
            : base(eventStream1, eventStream2, eventStream3, eventStream4, eventStream5, eventStream6, eventStream7, eventStream8, eventStream9, eventStream10, eventStream11, eventStream12, eventStream13, eventStream14)
        {
            this.eventStream15 = eventStream15;
        }

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.RegisterSubscription(this.eventStream15.Subscribe(this.HandleEvent));
        }

        public abstract void HandleEvent(TEvent15 @event);
    }
}