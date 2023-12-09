using MyEmployee.Client.Wpf.Models;

namespace MyEmployee.Client.Wpf.ViewModels
{
    //TODO: Тут реализовать Валидацию
    public class EmployeeProxy
    {
        internal readonly EmployeeModel model;

        public EmployeeProxy(EmployeeModel model)
        {
            this.model = model;
        }

        public string FirstName => model.FirstName;
        public int Id => model.Id;
    }
}
