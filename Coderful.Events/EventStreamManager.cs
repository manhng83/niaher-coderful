namespace Coderful.Events
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;

	/// <summary>
	/// Simple class to manage multiple IEventStream instances.
	/// </summary>
	public class EventStreamManager
	{
		private readonly Dictionary<Type, object> eventStreams = new Dictionary<Type, object>();

		/// <summary>
		/// Registers IEventStream instance in the internal collection.
		/// </summary>
		/// <typeparam name="T">Type of event that this IEventStream will handle.</typeparam>
		/// <param name="eventStream">IEventStream instance.</param>
		/// <remarks>This method needs to be called for every IEventStream that will be manager by 
		/// this EventStreamManager instance.</remarks>
		public void AddEventStream<T>(IEventStream<T> eventStream)
		{
			Contract.Requires(eventStream != null);

			this.eventStreams.Add(typeof(T), eventStream);
		}

		/// <summary>
		/// Registers IEventStream instance in the internal collection.
		/// </summary>
		/// <param name="eventStream">IEventStream instance.</param>
		/// <remarks>This method is a non-generic version of <see cref="AddEventStream{T}"/>.</remarks>
		public void AddEventStream(object eventStream)
		{
			Contract.Requires(eventStream != null);

			var eventType = eventStream.GetType().GenericTypeArguments[0];
			this.eventStreams.Add(eventType, eventStream);
		}

		/// <summary>
		/// Publishes an event. All subscribers to this event stream will be notified of the event.
		/// </summary>
		/// <param name="args">Event instance to publish.</param>
		public void Publish<T>(T args)
		{
			var stream = this.GetStream(args);

			stream.Publish(args);
		}

		/// <summary>
		/// Registers event handler. The handler will be invoked when the specified event is published.
		/// </summary>
		/// <typeparam name="T">Type of event.</typeparam>
		/// <param name="handler">Handler.</param>
		/// <returns>Instance of IDisposable which represents the subscription object. When a Dispose() method
		/// is called on this object, the handler will be unsubscribed from the associated IEventStream.</returns>
		/// <remarks>To release unused resources, subscription must be disposed when no longer needed.</remarks>
		public IDisposable RegisterHandler<T>(IEventHandler<T> handler)
		{
			Contract.Requires(handler != null);

			return this.EventStream<T>().Subscribe(handler.HandleEvent);
		}

		/// <summary>
		/// Registers event handler. The handler will be invoked when the specified event is published.
		/// </summary>
		/// <param name="handler">Handler object, which must implement <see cref="IEventHandler{TEvent}"/>.</param>
		/// <returns>Instance of IDisposable which represents the subscription object. When a Dispose() method
		/// is called on this object, the handler will be unsubscribed from the associated IEventStream.</returns>
		/// <remarks>To release unused resources, subscription must be disposed when no longer needed.</remarks>
		/// <remarks>This is a non-generic version of <see cref="RegisterHandler{T}"/>.</remarks>
		public IDisposable RegisterHandler(object handler)
		{
			Contract.Requires(handler != null);

			var interfaceType = handler.GetType().GetInterfaces()
				.SingleOrDefault(t => t.GetGenericTypeDefinition() == typeof(IEventHandler<>));

			if (interfaceType == null)
			{
				throw new ArgumentException(string.Format("Supplied parameter does not implement {0} interface.", nameof(IEventHandler<object>)));
			}

			var eventType = interfaceType.GenericTypeArguments[0];

			var stream = this.eventStreams[eventType];
			var streamType = stream.GetType();

			if (this.eventStreams == null)
			{
				throw new KeyNotFoundException(string.Format("Event stream for '{0}' was not found.", eventType.Name));
			}

			var handleEvent = handler.GetType().GetMethod(nameof(IEventHandler<object>.HandleEvent));
			var actionType = typeof(Action<>).MakeGenericType(eventType);
			var del = handleEvent.CreateDelegate(actionType, handler);

			var method = streamType.GetMethods().First(
				t =>
				t.Name == nameof(IEventStream<object>.Subscribe) &&
				t.GetParameters().FirstOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(Action<>));

			return (IDisposable)method.Invoke(stream, new object[] { del });
		}

		/// <summary>
		/// Subscribes a callback to the event stream. The callback will be invoked when
		/// an event is published to the stream.
		/// </summary>
		/// <param name="callback">Callback to invoke when an event a happens.</param>
		/// <returns>Instance of IDisposable which represents the subscription object. When a Dispose() method
		/// is called on this object, the callback will be unsubscribed from this IEventStream.</returns>
		/// <remarks>To release unused resources, subscription must be disposed when no longer needed.</remarks>
		public IDisposable Subscribe<T>(Action<T> callback)
		{
			Contract.Requires(callback != null);

			return this.EventStream<T>().Subscribe(callback);
		}

		/// <summary>
		/// Gets the IEventStream for the specified event type.
		/// </summary>
		/// <typeparam name="T">Type of event.</typeparam>
		/// <returns>IEventStream instance.</returns>
		protected IEventStream<T> EventStream<T>()
		{
			return this.eventStreams[typeof(T)] as IEventStream<T>;
		}

		/// <summary>
		/// Gets the IEventStream for the specified event type.
		/// </summary>
		/// <returns>IEventStream instance.</returns>
		protected object EventStream(Type type)
		{
			return this.eventStreams[type];
		}

		private IEventStream GetStream<T>(T args)
		{
			if (this.eventStreams.ContainsKey(typeof(T)))
			{
				return this.eventStreams[typeof(T)] as IEventStream<T>;
			}

			var type = args.GetType();

			var stream = this.eventStreams.SingleOrDefault(t => t.Key == type).Value as IEventStream;

			if (stream == null)
			{
				throw new EventInvocationException(string.Format("Event stream of type {0} was not registered.", type.FullName));
			}

			return stream;
		}
	}
}