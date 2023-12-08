using Microsoft.Extensions.DependencyInjection;
using MyEmployee.Client.Wpf.Abstractions;
using ReactiveUI;

namespace MyEmployee.Client.Wpf.ViewModels
{
    /// <summary>
    /// Реализация <see cref="IPageProvider"/>
    /// </summary>
    public class AppPageProvider : IPageProvider
    {
        private readonly IServiceProvider serviceProvider;

        public AppPageProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Возвращает главню страницу ( <see cref="EmployeeListViewModel"/> )
        /// </summary>       
        public IRoutableViewModel GetMainPage(IScreen? screen = null)
        {
            // Создаем через активатор
            return ActivatorUtilities.GetServiceOrCreateInstance<EmployeeListViewModel>(serviceProvider);
        }
    }
}
