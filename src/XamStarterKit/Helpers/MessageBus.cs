using System;
using Xamarin.Forms;

namespace XamStarterKit.Helpers
{
	public class MessageBus
	{
		private static readonly Lazy<MessageBus> LazyInstance = new Lazy<MessageBus>(() => new MessageBus(), true);

		private static MessageBus Instance => LazyInstance.Value;

		private MessageBus()
		{
		}

		public static void SendMessage(string message)
		{
			MessagingCenter.Send(Instance, message);
		}

		public static void SendMessage<TArgs>(string message, TArgs args)
		{
			MessagingCenter.Send(Instance, message, args);
		}

		public static void Subscribe(string message, Action callback)
		{
			MessagingCenter.Subscribe<MessageBus>(Instance, message, (bus) =>
			{
				callback?.Invoke();
			});
		}

		public static void Subscribe<TArgs>(string message, Action<TArgs> callback)
		{
			MessagingCenter.Subscribe<MessageBus, TArgs>(Instance, message, (bus, args) =>
			{
				callback?.Invoke(args);
			});
		}

		public static void Unsubscribe(string message)
		{
			MessagingCenter.Unsubscribe<MessageBus>(Instance, message);
		}

		public static void Unsubscribe<TArgs>(string message)
		{
			MessagingCenter.Unsubscribe<MessageBus, TArgs>(Instance, message);
		}
	}
}
