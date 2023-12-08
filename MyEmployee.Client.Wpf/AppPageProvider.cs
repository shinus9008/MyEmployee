using Microsoft.Extensions.DependencyInjection;
using MyEmployee.Client.Wpf.Abstractions;
using ReactiveUI;

namespace MyEmployee.Client.Wpf.ViewModels
{
    public class AppPageProvider : IPageProvider
    {
        private readonly IServiceProvider serviceProvider;

        public AppPageProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IRoutableViewModel GetMainPage(IScreen? screen = null)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance<EmployeeListViewModel>(serviceProvider);
        }
    }
}
