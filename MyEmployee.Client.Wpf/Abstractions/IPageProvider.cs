using ReactiveUI;

namespace MyEmployee.Client.Wpf.Abstractions
{
    /// <summary>
    /// Используется для навигации по страницам
    /// </summary>
    public interface IPageProvider
    {
        /// <summary>
        /// Возвращает ViewModel главной стрницы
        /// </summary>      
        public IRoutableViewModel GetMainPage(IScreen? screen = null);
    }
}
