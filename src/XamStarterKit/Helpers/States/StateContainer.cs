using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamStarterKit.Helpers.States
{
    [ContentProperty("Conditions")]
    public class StateContainer : ContentView
    {
        public List<StateCondition> Conditions { get; set; } = new List<StateCondition>();

        public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(object), typeof(StateContainer), null, BindingMode.Default, null, StateChanged);

		public object State
		{
			get => GetValue(StateProperty);
			set => SetValue(StateProperty, value);
		}

		public static void Init()
        {
            //for linker
        }

        private static async void StateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StateContainer)
            {
                var parent = (StateContainer)bindable;
                await parent.ChooseStateProperty(newValue);
            }
        }

        private async Task ChooseStateProperty(object newValue)
        {
            if (Conditions == null && Conditions?.Count == 0 || newValue == null)
            {
                Content = null;
                return;
            }

            try
            {
                var stateCondition = Conditions.FirstOrDefault(q => q.State != null && q.State.ToString().Equals(newValue?.ToString()));
				if (stateCondition == null)
					return;

                if (Content != null)
                {
                    await Content.FadeTo(0, 100U);
                    Content.IsVisible = false;
                    await Task.Delay(30); 
                }
                stateCondition.Content.Opacity = 0;
                Content = stateCondition.Content;
                Content.IsVisible = true;
                await Content.FadeTo(1);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"StateContainer ChooseStateProperty {newValue} error: {e}");
            }
        }
	}
}
