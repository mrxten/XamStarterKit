using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamStarterKit.Helpers {
    public delegate To ConvertMethod<From, To>(From value, Type targetType, object parameter, CultureInfo culture);
    public delegate To SimpleConvertMethod<From, To>(From value, object parameter);

    public class BasicConverter<From, To> : IValueConverter {
        public BasicConverter(
            SimpleConvertMethod<From, To> simpleConvertMethod = null,
            SimpleConvertMethod<To, From> simpleConvertBackMethod = null,
            ConvertMethod<From, To> convertMethod = null,
            ConvertMethod<To, From> convertBackMethod = null) {
            SimpleConvertMethod = simpleConvertMethod;
            SimpleConvertBackMethod = simpleConvertBackMethod;
            ConvertMethod = convertMethod;
            ConvertBackMethod = convertBackMethod;
        }

        public SimpleConvertMethod<From, To> SimpleConvertMethod { get; set; }
        public SimpleConvertMethod<To, From> SimpleConvertBackMethod { get; set; }
        public ConvertMethod<From, To> ConvertMethod { get; set; }
        public ConvertMethod<To, From> ConvertBackMethod { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                if (!ReferenceEquals(null, ConvertMethod))
                    return ConvertMethod((From)value, targetType, parameter, culture);

                if (!ReferenceEquals(null, SimpleConvertMethod))
                    return SimpleConvertMethod((From)value, parameter);
            }
            catch (Exception error) {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            try {
                if (!ReferenceEquals(null, ConvertBackMethod))
                    return ConvertBackMethod((To)value, targetType, parameter, culture);

                if (!ReferenceEquals(null, SimpleConvertBackMethod))
                    return SimpleConvertBackMethod((To)value, parameter);
            }
            catch (Exception error) {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
            return value;
        }

        public To Convert(From value, object parameter = null, To defValue = default(To)) {
            try {
                if (!ReferenceEquals(null, SimpleConvertMethod))
                    return SimpleConvertMethod(value, parameter);
            }
            catch (Exception error) {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
            return defValue;
        }
    }
}
