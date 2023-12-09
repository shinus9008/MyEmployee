using MyEmployee.Client.Wpf.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace MyEmployee.Client.Wpf.Views
{
    /// <summary>
    /// Interaction logic for EmployeeListView.xaml
    /// </summary>
    public partial class EmployeeListView : ReactiveUserControl<EmployeeListViewModel>
    {
        public EmployeeListView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                DataContext = ViewModel;

                // Когда вюшка создается привязываем свойства
                this.BindCommand(ViewModel, x => x.GoToCreate, x => x.GoToCreateButton)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoToEdit, x => x.GoToEditButton)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.GoToDelete, x => x.GoToDeleteButton)
                    .DisposeWith(disposables);
            });
        }
    }
}
