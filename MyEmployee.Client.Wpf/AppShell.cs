using MyEmployee.Client.Wpf.Abstractions;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Главная вюшка. Отвечает за навигацию по IRoutableViewModel
    /// </summary>
    /// <remarks>
    /// Отвечает за навигацию по ViewModels
    /// </remarks>
    public class AppShell : ReactiveObject, IScreen
    {
        //TOOD: Проверить время жизни страниц при навигации!

        public AppShell(IPageProvider mainPageProvider)
        {
            // Создаем навигатор
            Router = new RoutingState();

            GoNext = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    // Тут может быть какаято логика навигации,
                    // но мы просто перейдем на главную страницу

                    var mainPage = mainPageProvider.GetMainPage(this);

                    return Router.Navigate.Execute(mainPage);
                });

            // Мы можем вернуться обратно, если стек не пуст
            var canGoBack = this
                .WhenAnyValue(x => x.Router.NavigationStack.Count)
                .Select(count => count > 0);

            // Создаем команду перехода обратно
            GoBack = ReactiveCommand.CreateFromObservable(
                () => Router.NavigateBack.Execute(Unit.Default),
                canGoBack);
        }


        /// <summary>
        /// Управляет навигацией
        /// </summary>
        public RoutingState Router { get; }

        
        public ReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    }
}
