namespace Coderful.Events.Test.Domain
{
	public class MyEventHandler : EventHandler<string, int>
	{
		public MyEventHandler(IEventStream<string> eventStream, IEventStream<int> intStream) : base(eventStream, intStream)
		{
		}
        
	    public override void HandleEvent(string @event)
	    {
            // Do something.
	    }

	    public override void HandleEvent(int @event)
	    {
	        // Do something.
	    }
	}
}
