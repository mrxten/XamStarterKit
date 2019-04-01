using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamStarterKit.Helpers.States {
    [ContentProperty("Conditions")]
    public class StateContainer : ContentView {
        public static readonly BindableProperty ConditionsProperty =
            BindableProperty.Create(
                nameof(Conditions),
                typeof(IList<StateCondition>),
                typeof(StateContainer),
                new List<StateCondition>(),
                BindingMode.Default,
                null,
                CurrentViewPropertyChanged);

        public IList<StateCondition> Conditions {
            get => (IList<StateCondition>)GetValue(ConditionsProperty);
            set => SetValue(ConditionsProperty, value);
        }

        public static readonly BindableProperty StateProperty =
            BindableProperty.Create(
                nameof(State),
                typeof(object),
                typeof(StateContainer),
                null,
                BindingMode.Default,
                null,
                CurrentViewPropertyChanged);

        public object State {
            get => GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        static async void CurrentViewPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
            if (bindable is StateContainer sc) {
                await sc.ChooseStateProperty(sc.State);
            }
        }

        async Task ChooseStateProperty(object newValue) {
            if (Conditions == null && Conditions?.Count == 0 || newValue == null) {
                return;
            }

            try {
                var stateCondition = Conditions.FirstOrDefault(q => q.State != null && q.State.ToString().Equals(newValue?.ToString()));
                if (stateCondition == null)
                    return;

                if (Content != null) {
                    await Content.FadeTo(0, 100U);
                    Content.IsVisible = false;
                    await Task.Delay(30);
                }
                stateCondition.Content.Opacity = 0;
                Content = stateCondition.Content;
                Content.IsVisible = true;
                await Content.FadeTo(1);
            }
            catch (Exception e) {
                Debug.WriteLine($"StateContainer ChooseStateProperty {newValue} error: {e}");
            }
        }
    }
}
