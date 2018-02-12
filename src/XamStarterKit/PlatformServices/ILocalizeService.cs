using System.Globalization;

namespace XamStarterKit.PlatformServices
{
    public interface ILocalizeService
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
