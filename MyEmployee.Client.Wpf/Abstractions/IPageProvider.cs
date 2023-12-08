using ReactiveUI;

namespace MyEmployee.Client.Wpf.Abstractions
{
    public interface IPageProvider
    {
        public IRoutableViewModel GetMainPage(IScreen? screen = null);
    }
}
