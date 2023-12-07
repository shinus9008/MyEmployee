using ReactiveUI;
using System.Reactive.Disposables;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public AppBootstrapper AppBootstrapper { get; protected set; }

        public MainWindow()
        {
            InitializeComponent();
            AppBootstrapper = new AppBootstrapper();
            ViewModel       = new MainViewModel();
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