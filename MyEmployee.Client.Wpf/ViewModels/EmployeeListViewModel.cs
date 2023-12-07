using DynamicData;
using MyEmployee.Client.Wpf.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf.ViewModels
{
    public class EmployeeListViewModel : ViewModelBase
    {
        FakeEmployeeService fakeEmployeeService = new FakeEmployeeService();
        private readonly ReadOnlyObservableCollection<EmployeeViewModel> _item;
        private object lifteTime;

        public EmployeeListViewModel(            
            IScreen screen, 
            IScheduler? scheduler = null) 
            : base(screen, "Items")
        {

            

            lifteTime = 


            fakeEmployeeService.Connect
                 .Transform(FactoryMethod)
                 .ObserveOn(scheduler ?? RxApp.MainThreadScheduler) //TODO: Удобен для тестирования
                 .Bind(out _item)
                 .Subscribe();
        }

        


        private EmployeeViewModel FactoryMethod(EmployeeModel model)
        {
            return new EmployeeViewModel()
            {
                Id = model.Id,
            };
        }


        public ReadOnlyObservableCollection<EmployeeViewModel> Items => _item;
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
