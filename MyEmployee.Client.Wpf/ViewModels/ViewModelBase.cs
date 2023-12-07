using ReactiveUI;

namespace MyEmployee.Client.Wpf.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IRoutableViewModel
    {
        public ViewModelBase(IScreen screen, string url)
        {
            HostScreen     = screen;
            UrlPathSegment = url;
        }

        public string UrlPathSegment { get; }

        public IScreen HostScreen { get; }
    }
}
