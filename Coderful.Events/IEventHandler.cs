namespace Coderful.Events
{
	/// <summary>
    /// Interface for a class that can handle events of a certain type.
    /// </summary>
	public interface IEventHandler<in TEvent>
	{
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="event">TEvent instance.</param>
        void HandleEvent(TEvent @event);
	}
}
