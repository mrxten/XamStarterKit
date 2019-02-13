using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamStarterKit.Extensions.FastUI {
    public static class BuildUiExtensions {
        public static T Do<T>(this T view, Action<T> action)
            where T : View {
            action?.Invoke(view);
            return view;
        }

        public static T Bind<T>(
           this T view,
           BindableProperty prop,
           string path,
           BindingMode mode = BindingMode.Default,
           IValueConverter converter = null,
           object converterParameter = null,
           string stringFormat = null,
           object source = null)
            where T : BindableObject {
            var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
            view.SetBinding(prop, binding);
            return view;
        }

        public static T Add<T>(this T layout, params View[] views) where T : Layout<View> {
            foreach (var view in views) {
                layout.Children.Add(view);
            }
            return layout;
        }

        public static T AddTap<T>(this T view, Action action)
            where T : View {
            view.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(action) });
            return view;
        }

        public static T AddTap<T>(this T view, ICommand command, object commandParameter = null)
            where T : View {
            view.GestureRecognizers.Add(new TapGestureRecognizer { Command = command, CommandParameter = commandParameter });
            return view;
        }

        public static T AddTap<T>(this T view, string commandPath, object commandParameter = null, string commandParameterPath = null)
            where T : View {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, commandPath);
            if (!string.IsNullOrEmpty(commandParameterPath))
                tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandParameterProperty, commandParameterPath);
            else
                tapGestureRecognizer.CommandParameter = commandParameter;

            view.GestureRecognizers.Add(tapGestureRecognizer);
            return view;
        }

        public static Button AddButtonCommand(this Button btn, string commandPath = null, string commandParameterPath = null) {
            if (!string.IsNullOrEmpty(commandPath))
                btn.Bind(Button.CommandProperty, commandPath);
            if (!string.IsNullOrEmpty(commandParameterPath))
                btn.Bind(Button.CommandParameterProperty, commandParameterPath);
            return btn;
        }

        public static T ToVariable<T>(this T self, out T variable) 
            where T : class {
            variable = self;
            return self;
        }
    }
}
