namespace XamStarterKit.ViewModels.Abstractions
{
    public interface ICancellable
    {
        void CancellAll();

        void Cancel(object obj);
    }
}