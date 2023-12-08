using Microsoft.Extensions.DependencyInjection;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Observables;
using MyEmployee.Client.Wpf.Services;
using MyEmployee.Client.Wpf.SmaleTasks;
using MyEmployee.Client.Wpf.ViewModels;
using MyEmployee.Client.Wpf.Views;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Конфигурация сервисов. 
    /// Используюся MS DI.
    /// </summary>
    public class AppBootstrapper
    {
        /// <summary>
        /// DI Контейнер
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyResolver"></param>
        public AppBootstrapper(IMutableDependencyResolver? dependencyResolver = null)
        {
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            //1 Создаем коллекцию сервисов
            var serviceCollection = GetServiceCollection();

            //2 Передаем коллекцию в сплат (Чтобы сплат смог зарегистрировал свои свервисы)
            serviceCollection.UseMicrosoftDependencyResolver();

            //3) Регистрация сервисов через сплат SPLAT            
            dependencyResolver.InitializeSplat();         //-- Splat
            dependencyResolver.InitializeReactiveUI();    //-- ReactiveUI         
            dependencyResolver.RegisterViewsForViewModels(Assembly.GetExecutingAssembly()); // Находит все IViewFor в сборке и регистрирует в контейнере

            //4) Регистрация сервисов              
            Configure(serviceCollection);

            //5)  Создаем Провайдера севисов
            ServiceProvider = GetServiceProvider(serviceCollection);
            
            //6) Передаем провайдер в SPLAT
            ServiceProvider.UseMicrosoftDependencyResolver();
        }

        /// <summary>
        /// Создает <see cref="IServiceCollection"/>
        /// </summary>      
        protected virtual IServiceCollection GetServiceCollection()
        {
            return new ServiceCollection();
        }
        
        /// <summary>
        /// Создает <see cref="IServiceProvider"/>
        /// </summary>    
        protected virtual IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            return serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Конфигурация сервисов
        /// </summary>
        /// <param name="serviceCollection"></param>
        private void Configure(IServiceCollection serviceCollection)
        {
            //
            serviceCollection.AddSingleton<IScreen, AppShell>();
            serviceCollection.AddSingleton<IPageProvider, AppPageProvider>();

            //
            serviceCollection.AddSingleton<EmployeeListViewModel>();

            //TODO: Регистратор через splat чет не заработал
            serviceCollection.AddTransient<IViewFor<EmployeeListViewModel>, EmployeeListView>();

            
            // Вариант #1 (Ленивое заполнение; В памяти)
            {
                //serviceCollection.AddTransient<IEmployeeObservable, FakeEmployeeObservable>();
            }

            // Вариант #2 ()
            {
                serviceCollection.AddSingleton<IEmployeeCache,      EmployeeCache>();
                serviceCollection.AddTransient<IEmployeeObservable, LoadEmployeeObservable>();
                serviceCollection.AddTransient<IEmployeeService,    GrpcEmployeeService>();
            }


            serviceCollection.AddTransient<LoadingSmaleTask>();


            // HTTP and GRPC client registrations 
            //TODO: не заработал через коллекцию надо чегото настраивать
            //serviceCollection.AddGrpcClient<API.WorkerIntegration.WorkerIntegrationClient>(o => o.Address = new("https://basket-api"));
        }
    }
}
