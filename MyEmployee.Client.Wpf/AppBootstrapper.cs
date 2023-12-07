using ReactiveUI;
using Splat;
using System.Reflection;

namespace MyEmployee.Client.Wpf
{
    public class AppBootstrapper
    {
        public AppBootstrapper(IMutableDependencyResolver? dependencyResolver = null)
        {
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            RegisterParts(dependencyResolver);

            // TODO: This is a good place to set up any other app startup tasks
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            // Находит все IViewFor в сборке и регистрирует в контейнере
            dependencyResolver.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());

            // Переопределяем логику навигации
            // dependencyResolver.RegisterLazySingleton(() => new AppViewLocator(), typeof(IViewLocator));
        }
    }
}
