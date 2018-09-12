using System;
using Xamarin.Forms;

namespace XamStarterKit.Helpers {
    public class MessageBus {
        private static readonly Lazy<MessageBus> LazyInstance = new Lazy<MessageBus>(() => new MessageBus(), true);

        private static MessageBus Instance => LazyInstance.Value;

        private MessageBus() {
        }

        public static void SendMessage(object message) {
            MessagingCenter.Send(Instance, message.ToString());
        }

        public static void SendMessage<TArgs>(object message, TArgs args) {
            MessagingCenter.Send(Instance, message.ToString(), args);
        }

        public static void Subscribe(object subscriber, object message, Action callback) {
            MessagingCenter.Subscribe<MessageBus>(subscriber, message.ToString(), (bus) => {
                callback?.Invoke();
            });
        }

        public static void Subscribe<TArgs>(object subscriber, object message, Action<TArgs> callback) {
            MessagingCenter.Subscribe<MessageBus, TArgs>(subscriber, message.ToString(), (bus, args) => {
                callback?.Invoke(args);
            });
        }

        public static void Unsubscribe(object subscriber, object message) {
            MessagingCenter.Unsubscribe<MessageBus>(subscriber, message.ToString());
        }

        public static void Unsubscribe<TArgs>(object subscriber, object message) {
            MessagingCenter.Unsubscribe<MessageBus, TArgs>(subscriber, message.ToString());
        }
    }
}