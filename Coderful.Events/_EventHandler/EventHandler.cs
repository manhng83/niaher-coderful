namespace Coderful.Events
{
	using System;
	using System.Collections.Generic;

    /// <summary>
    /// Base class for an event handler which allows to start and stop listening to events
    /// at any time. The main purpose of this base class is to provide functionality that manages
    /// event subscriptions and disposes of them properly when no longer needed.
    /// </summary>
    public abstract class EventHandler
    {
        /// <summary>
        /// Gets the list of active subscriptions.
        /// </summary>
        /// <remarks>Whenever a subscription is created by an instance of this class, it must be added to 
        /// this list. Failure to do so,  will result in the "Stop" method not removing all created 
        /// subscription.</remarks>
        private readonly List<IDisposable> subscriptions = new List<IDisposable>();

        /// <summary>
        /// Creates subscriptions for all events that this IEventHandler will listen to.
        /// </summary>
        /// <remarks>When overriding this method, it is required that each subscription that is created by this
        /// method is registered using "RegisterSubscription" method.</remarks>
        public abstract void Start();

        /// <summary>
        /// Disposes of all subscriptions and stops processing any future events.
        /// </summary>
        public void Stop()
        {
            foreach (var subscription in this.subscriptions)
            {
                subscription.Dispose();
            }

            this.subscriptions.Clear();
        }

        public void Dispose()
        {
            this.Stop();
        }

        /// <summary>
        /// Registers subscription in the internal list, so that it can be disposed of properly
        /// when it is no longer needed.
        /// </summary>
        /// <param name="subscription">IDisposable instance.</param>
        protected void RegisterSubscription(IDisposable subscription)
        {
            this.subscriptions.Add(subscription);
        }
    }
}
