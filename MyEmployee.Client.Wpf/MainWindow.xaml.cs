using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<AppShell>
    {
        public AppBootstrapper AppBootstrapper { get; protected set; }

        public MainWindow()
        {
            InitializeComponent();

            // Иницилизируем DI
            AppBootstrapper = new AppBootstrapper();

            // Получаем главную View
            ViewModel       = AppBootstrapper.ServiceProvider.GetService<IScreen>() as AppShell; //TODO: Сделать красиво!


            this.WhenActivated(disposables =>
            {
                // Когда вюшка создается привязываем свойства
                this.OneWayBind (ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoNext, x => x.GoNextButton)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoBack, x => x.GoBackButton)
                    .DisposeWith(disposables);
            });
        }
    }
}