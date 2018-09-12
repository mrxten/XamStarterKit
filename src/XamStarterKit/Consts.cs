using System;
namespace XamStarterKit {
    public static class KitMessages {
        public const string NavigationPushMessage = nameof(NavigationPushMessage);
        public const string NavigationPopMessage = nameof(NavigationPopMessage);
    }

    public enum NavigationMode {
        Normal,
        Modal,
        Custom,
        Root,
        PopUp
    }

    public enum KitPageState {
        Clean,
        Loading,
        Empty,
        Normal,
        Error
    }
}
