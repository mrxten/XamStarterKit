using Xamarin.Forms;

namespace XamStarterKit.Helpers.States
{
    [ContentProperty("Content")]
    public class StateCondition : View
    {
        public object State { get; set; }
        public View Content { get; set; }
    }
}
