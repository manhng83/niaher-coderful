namespace Coderful.Events
{
	using System;
	using System.Reactive.Subjects;

	/// <summary>
	/// Implementation of IEventStream.
	/// </summary>
	/// <typeparam name="TArgument">Type of event that this IEventStream will publish.</typeparam>
	public class EventStream<TArgument> : IEventStream<TArgument>
	{
		private readonly Subject<TArgument> observable;

		/// <summary>
		/// Instantiates a new instance of EventStream class.
		/// </summary>
		public EventStream()
		{
			this.observable = new Subject<TArgument>();
		}

		/// <summary>
		/// Publishes an event. All subscribers to this event stream will be notified of the event.
		/// </summary>
		/// <param name="args">Event instance to publish.</param>
		public void Publish(TArgument args)
		{
			this.observable.OnNext(args);
		}

		/// <summary>
		/// Publishes an event. All subscribers to this event stream will be notified of the event.
		/// </summary>
		/// <param name="args">Event instance to publish.</param>
		public void Publish(object args)
		{
			this.Publish((TArgument)args);
		}

		/// <summary>
		/// Subscribes a callback to the event stream. The callback will be invoked when
		/// an event is published to the stream.
		/// </summary>
		/// <param name="callback">Callback to invoke when an event a happens.</param>
		/// <returns>Instance of IDisposable which represents the subscription object. When a Dispose() method
		/// is called on this object, the callback will be unsubscribed from this IEventStream.</returns>
		/// <remarks>To release unused resources, subscription must be disposed when no longer needed.</remarks>
		public IDisposable Subscribe(Action<TArgument> callback)
		{
			return this.observable.Subscribe(callback);
		}

		/// <summary>
		/// Subscribes an observer to the event stream. The observer will be notifier when
		/// an event is published to the stream.
		/// </summary>
		/// <param name="observer">Observer to invoke when an event a happens.</param>
		/// <returns>Instance of IDisposable which represents the subscription object. When a Dispose() method
		/// is called on this object, the callback will be unsubscribed from this IEventStream.</returns>
		/// <remarks>To release unused resources, subscription must be disposed when no longer needed.</remarks>
		public IDisposable Subscribe(IObserver<TArgument> observer)
		{
			return this.observable.Subscribe(observer);
		}

		/// <summary>
		/// Unsubscribes previously subscribed callback.
		/// </summary>
		/// <param name="subscription">Subscription object which was returned from the "Subscribe" method.</param>
		public void Unsubscribe(IDisposable subscription)
		{
			subscription.Dispose();
		}
	}
}