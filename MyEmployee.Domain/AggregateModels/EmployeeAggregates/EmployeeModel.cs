using MyEmployee.Domain.SeedWork;
using System;

namespace MyEmployee.Domain.AggregateModels.EmployeeAggregates
{
    //TODO: Тут должны быть бизнес правила работника
    public class EmployeeModel : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        public bool HaveChildren { get; set; }

        public EmployeeSex Sex { get; set; }
    }

    public enum EmployeeSex
    {
        Male,
        Female,
    }
}
