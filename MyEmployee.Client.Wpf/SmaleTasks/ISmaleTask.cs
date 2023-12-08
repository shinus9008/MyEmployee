namespace MyEmployee.Client.Wpf.SmaleTasks
{
    /// <summary>
    /// Инфроструктура действия.
    /// </summary>
    public interface ISmaleTask
    {
        Task DoAsync(CancellationToken token);
    }
}