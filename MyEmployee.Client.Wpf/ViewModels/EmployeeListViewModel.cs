﻿using DynamicData;
using MyEmployee.Client.Wpf.Abstractions;
using MyEmployee.Client.Wpf.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MyEmployee.Client.Wpf.ViewModels
{
    public class EmployeeListViewModel : ViewModelBase
    {
        private readonly ReadOnlyObservableCollection<EmployeeViewModel> _item;
        private readonly IScreen screen;
        private object lifteTime;

        public EmployeeListViewModel(            
            IScreen screen,
            IEmployeeObservable employeeObservableCache,
            IScheduler? scheduler = null) 
            : base(screen, "Items")
        {
            GoToCreate = ReactiveCommand.CreateFromObservable(() => screen.Router.Navigate.Execute(GetEmployeeValidatebleViewModel()));
            GoToEdit   = ReactiveCommand.CreateFromObservable(() => screen.Router.Navigate.Execute(GetEmployeeValidatebleViewModel()));
            GoToDelete = ReactiveCommand.CreateFromObservable(() => screen.Router.Navigate.Execute(GetEmployeeValidatebleViewModel()));

            //TODO: Отписаться когда окно закроется
            lifteTime =
            employeeObservableCache.ObservableCache
                 .Connect()
                 .Transform(FactoryMethod)
                 .ObserveOn(scheduler ?? RxApp.MainThreadScheduler) //TODO: Можно сделать провайдер. 
                 .Bind(out _item)
                 .Subscribe();
            this.screen = screen;
        }

        /// <summary>
        /// Обновить список сотрудников
        /// </summary>
        public ReactiveCommand<Unit, Unit> Update { get; }

        /// <summary>
        /// Навигация на страницу создания
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> GoToCreate { get; }

        /// <summary>
        /// Навигация на страницу редактирования
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> GoToEdit { get; }

        /// <summary>
        /// Навигация на страницу удаления
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> GoToDelete { get; }


        private EmployeeViewModel FactoryMethod(EmployeeModel model)
        {
            return new EmployeeViewModel()
            {
                Id = model.Key,
            };
        }
        private EmployeeValidatebleViewModel GetEmployeeValidatebleViewModel()
        {
            return new EmployeeValidatebleViewModel(screen, "Create")
            {

            };
        }
        public ReadOnlyObservableCollection<EmployeeViewModel> Items => _item;



        
    }

   
    public class EmployeeViewModel
    {
        public int Id { get; set; }
    }

    public class EmployeeValidatebleViewModel : ViewModelBase
    {
        public EmployeeValidatebleViewModel(IScreen screen, string urlAction) : base(screen, "Employee/" + urlAction)
        {
        }

        public int Id { get; set; }
    }
}
