namespace Coderful.Events.Test
{
	using System;
	using TechTalk.SpecFlow;
	using Xunit;

	[Binding]
	public class EventStreamSteps
	{
		public EventStreamSteps()
		{
			Context = new Parameters { EventStream = new EventStream<string>() };
		}

		private static Parameters Context
		{
			get
			{
				return ScenarioContext.Current.Get<Parameters>();
			}

			set
			{
				ScenarioContext.Current.Set(value);
			}
		}
		
		[Given(@"an action has been subscribed to an event stream")]
		public void GivenAnActionHasBeenSubscribedToTheEventStream()
		{
			Context.Action = s =>
			{
				Context.ActionCallCount++;
				Context.LastArgumentReceived = s;
			};

			Context.Subscription = Context.EventStream.Subscribe(Context.Action);
		}

		[When(@"an event is published")]
		public void WhenAnEventIsPublished()
		{
			Context.EventStream.Publish("something just happened");
		}

		[When(@"an event is published with argument (.*)")]
		public void WhenAnEventIsPublishedWithArgument(string arg)
		{
			Context.EventStream.Publish(arg);
		}

		[When(@"action is unsubscribed")]
		public void WhenActionIsUnsubscribed()
		{
			Context.Subscription.Dispose();
			Context.Subscription = null;
		}
		
		[Then(@"the callback must be invoked")]
		public void ThenTheCallbackMustBeInvoked()
		{
			Assert.Equal(1, Context.ActionCallCount);
		}

		[Then(@"the callback must be invoked with argument (.*)")]
		public void ThenTheCallbackMustBeInvokedWithArgument(string arg)
		{
			Assert.Equal(arg, Context.LastArgumentReceived);
		}

		[Then(@"the action must not be invoked")]
		public void ThenTheActionMustNotBeInvoked()
		{
			Assert.Equal(0, Context.ActionCallCount);
		}

		private class Parameters
		{
			public IEventStream<string> EventStream { get; set; }
			public Action<string> Action { get; set; }
			public IDisposable Subscription { get; set; }
			public int ActionCallCount { get; set; }
			public string LastArgumentReceived { get; set; }
		}
	}
}