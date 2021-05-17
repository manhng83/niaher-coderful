namespace Coderful.Events
{
	using System;

	public static class Extensions
	{
		public static void Subscribe<T>(this IObservable<T> observable, IEventHandler<T> handler)
		{
			observable.Subscribe(handler.HandleEvent);
		}
	}
}
