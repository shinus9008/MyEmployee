using ReactiveUI;

namespace MyEmployee.Client.Wpf.ViewModels
{
    public class EmployeeListViewModel : ViewModelBase
    {
        public EmployeeListViewModel(IScreen screen) 
            : base(screen, "Items")
        {
        }
    }
}
