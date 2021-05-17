namespace Coderful.Events
{
	using System;

	/// <summary>
    /// Represents an event stream through which events can be published.
    /// </summary>
    /// <typeparam name="TArgument">Type of event.</typeparam>
	public interface IEventStream<TArgument> : IObservable<TArgument>, IEventStream
	{
        /// <summary>
        /// Publishes an event. All subscribers to this event stream will be notified of the event.
        /// </summary>
        /// <param name="args">Event instance to publish.</param>
		void Publish(TArgument args);

        /// <summary>
        /// Subscribes a callback to the event stream. The callback will be invoked when
        /// an event is published to the stream.
        /// </summary>
        /// <param name="callback">Callback to invoke when an event a happens.</param>
        /// <returns>Instance of IDisposable which represents the subscription object. When a Dispose() method
        /// is called on this object, the callback will be unsubscribed from this IEventStream.</returns>
        /// <remarks>To release unused resources, subscription must be disposed when no longer needed.</remarks>
		IDisposable Subscribe(Action<TArgument> callback);
	}

	public interface IEventStream
	{
		/// <summary>
		/// Publishes an event. All subscribers to this event stream will be notified of the event.
		/// </summary>
		/// <param name="args">Event instance to publish.</param>
		void Publish(object args);
	}
}