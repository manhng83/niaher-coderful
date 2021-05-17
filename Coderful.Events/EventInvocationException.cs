namespace Coderful.Events
{
	using System;
	using System.Runtime.Serialization;

	[Serializable]
	public class EventInvocationException : Exception
	{
		public EventInvocationException(string message)
			: base(message)
		{
		}

		public EventInvocationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected EventInvocationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}