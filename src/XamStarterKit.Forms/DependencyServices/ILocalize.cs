using System.Globalization;

namespace XamStarterKit.Forms.DependencyServices
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
