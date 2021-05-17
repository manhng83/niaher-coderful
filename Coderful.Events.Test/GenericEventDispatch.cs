namespace Coderful.Events.Test
{
	using System;
	using Xunit;

	public class GenericEventDispatchTest
	{
		[Fact]
		public void Works()
		{
			var em = new EventStreamManager();
			em.AddEventStream(new EventStream<MyEvent>());

			IDomainEvent e = new MyEvent();
			em.Publish(e);
		}

		[Fact]
		public void NonGenericRegistrationWorks()
		{
			int count = 0;

			var em = new EventStreamManager();
			var stream = new EventStream<MyEvent>();
			var myEventHandler = new MyEventHandler(stream, () => count++);

			em.AddEventStream((object)stream);
			em.RegisterHandler((object)myEventHandler);

			IDomainEvent e = new MyEvent();
			em.Publish(e);

			Assert.True(count == 1);
		}

		public class MyEvent : IDomainEvent
		{
		}

		public interface IDomainEvent
		{
		}

		public class MyEventHandler : Coderful.Events.EventHandler<MyEvent>
		{
			private readonly Action action;

			public MyEventHandler(IEventStream<MyEvent> eventStream, Action action) : base(eventStream)
			{
				this.action = action;
			}

			public override void HandleEvent(MyEvent @event)
			{
				this.action();
			}
		}
	}
}