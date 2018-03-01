﻿using System;
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

		public static void Subscribe(object subscriber, string message, Action callback)
		{
			MessagingCenter.Subscribe<MessageBus>(subscriber, message, (bus) =>
			{
				callback?.Invoke();
			});
		}

		public static void Subscribe<TArgs>(object subscriber, string message, Action<TArgs> callback)
		{
			MessagingCenter.Subscribe<MessageBus, TArgs>(subscriber, message, (bus, args) =>
			{
				callback?.Invoke(args);
			});
		}

		public static void Unsubscribe(object subscriber, string message)
		{
			MessagingCenter.Unsubscribe<MessageBus>(subscriber, message);
		}

		public static void Unsubscribe<TArgs>(object subscriber, string message)
		{
			MessagingCenter.Unsubscribe<MessageBus, TArgs>(subscriber, message);
		}
	}
}
