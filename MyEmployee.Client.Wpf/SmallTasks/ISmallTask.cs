namespace MyEmployee.Client.Wpf.SmaleTasks
{
    /// <summary>
    /// Инфроструктура действия.
    /// </summary>
    public interface ISmallTask
    {
        Task DoAsync(CancellationToken token);
    }
}