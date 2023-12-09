using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using MyEmployee.Client.Wpf.Services;
using MyEmployee.Shared;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf.ViewModels
{
    public class EmployeeListViewModel : ViewModelBase
    {
        private readonly ReadOnlyObservableCollection<EmployeeProxy> _item;      

        public EmployeeListViewModel(            
            IScreen screen,
            IEmployeeObservable employeeObservableCache, 
            IEmployeeService employeeService,
            IScheduler? scheduler = null) 
            : base(screen, "Items")
        {
            //TODO: Отписаться когда окно закроется
            var lifteTime =
            employeeObservableCache.Connect                               
                 .Transform(model => new EmployeeProxy(model)) 
                 .ObserveOn(scheduler ?? RxApp.MainThreadScheduler) //TODO: Можно сделать провайдер. 
                 .Bind(out _item)
                 .DisposeMany()
                 .Subscribe();

            // Коианды можно выполнять когда выбран элемеент списка!
            // TODO: и сесли не выполняется другая команда?
            var canInvoke = this.WhenAnyValue(x => x.Selected)
                                .Select(x => x == null ? false : true);

            // Поидее должна быть навигация на страницы,
            // но я не успеваю поэтому просто ранодомайзер
            // Каждой команде можно подвести обработчик исключений выводящий все в диалог
            // В данный момент диалог системный обработка в главном окне
            GoToCreate = ReactiveCommand.CreateFromTask(async () =>
            {
                // 
                var model = new EmployeePoco()
                {
                    FirstName = RandomHelper.RandomString(10),
                    LastName  = RandomHelper.RandomString(10),
                };
                await Task.Delay(5000);

                await employeeService.CreateEmployee(model).ConfigureAwait(true);

                //

            });
            GoToEdit   = ReactiveCommand.CreateFromTask(async () =>
            {
                // 
                var model = Selected!.model;
                model.FirstName = RandomHelper.RandomString(10);
                model.LastName = RandomHelper.RandomString(10);

                await employeeService.UpdateEmployee(model).ConfigureAwait(true);

                //

            }, canInvoke);
            GoToDelete = ReactiveCommand.CreateFromTask(async () =>
            {
                // 
                var model = Selected!.model;                              

                await employeeService.DeleteEmployee(model).ConfigureAwait(true);

                //

            }, canInvoke);
        }

        [Reactive]
        public EmployeeProxy? Selected { get; set; }

        /// <summary>
        /// Навигация на страницу создания
        /// </summary>
        public ReactiveCommand<Unit, Unit> GoToCreate { get; }

        /// <summary>
        /// Навигация на страницу редактирования
        /// </summary>
        public ReactiveCommand<Unit, Unit> GoToEdit { get; }

        /// <summary>
        /// Навигация на страницу удаления
        /// </summary>
        public ReactiveCommand<Unit, Unit> GoToDelete { get; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyObservableCollection<EmployeeProxy> Items => _item;
    }
}
