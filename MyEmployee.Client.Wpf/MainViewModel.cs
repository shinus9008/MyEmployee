using MyEmployee.Client.Wpf.ViewModels;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Главная вюшка
    /// </summary>
    /// <remarks>
    /// Отвечает за навигацию по ViewModels
    /// </remarks>
    public class MainViewModel : ReactiveObject, IScreen
    {
        DynamicData.SourceCache<EmployeeModel, int> cache = new DynamicData.SourceCache<EmployeeModel, int>(i => i.Id);

        public MainViewModel()
        {
            // Создаем навигатор
            Router = new RoutingState();

            GoNext = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new EmployeeListViewModel(this)));

            var canGoBack = this
                .WhenAnyValue(x => x.Router.NavigationStack.Count)
                .Select(count => count > 0);

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

    public class EmployeeModel
    {
        public int Id { get; set; }
    }

    public class EmployeeViewModel
    {
        public int Id { get; set; }
    }
}
