using System;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamStarterKit.Localization {
    [ContentProperty("Text")]
    public class TranslateMarkupExtension : IMarkupExtension {
        static CultureInfo _ci;
        static ResourceManager _resourceManager;

        public static void Init(CultureInfo ci, ResourceManager resourceManager) {
            _ci = ci;
            _resourceManager = resourceManager;
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider) {
            if (Text == null)
                return "";

            var translation = _resourceManager?.GetString(Text, _ci);

            if (translation == null) {
                translation = Text;
            }
            return translation;
        }
    }
}
