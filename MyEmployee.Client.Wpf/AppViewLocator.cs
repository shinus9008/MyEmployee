﻿using MyEmployee.Client.Wpf.ViewModels;
using MyEmployee.Client.Wpf.Views;
using ReactiveUI;

namespace MyEmployee.Client.Wpf
{
    /// <summary>
    /// Определяет логику создания View для ViewModel
    /// </summary>
    public class AppViewLocator : IViewLocator
    {
        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            //
            throw new Exception($"Не найдена View для ViewModel {typeof(T).Name}.");
        }
    }
}
