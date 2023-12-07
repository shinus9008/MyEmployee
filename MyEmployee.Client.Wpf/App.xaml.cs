using ReactiveUI;
using Splat;
using System.Reflection;
using System.Windows;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //TODO: ДОбавить обработчик исключений!

        //TODO: ДОбавить обработчик исключений при навигации!

        public App()
        {
            // Находит все IViewFor в сборке и регистрирует в контейнере
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());



            //Locator.CurrentMutable.RegisterLazySingleton(() => new AppViewLocator(), typeof(IViewLocator));
        }
    }

}
