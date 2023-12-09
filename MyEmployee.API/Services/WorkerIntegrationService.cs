using Grpc.Core;
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;
using System.Collections.Concurrent;

namespace MyEmployee.API.Services
{
    public class WorkerIntegrationService: WorkerIntegration.WorkerIntegrationBase
    {
        private readonly IEmployeeRepository employeeRepository;

        public WorkerIntegrationService(
            IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        // TODO: Авторизаййия и аутентификация не настроенв

        public override Task GetEmployeeList(EmployeeListRequest request, IServerStreamWriter<EmployeeListReply> responseStream, ServerCallContext context)
        {
            return base.GetEmployeeList(request, responseStream, context);
        }
        public override async Task GetEmployeeStream(EmptyMessage request, IServerStreamWriter<EmployeeReply> responseStream, ServerCallContext context)
        {
            await foreach (var employee in employeeRepository.GetAllAsync())
            {
                var dto = Map(employee);

                await responseStream.WriteAsync(dto);
            }
        }
        
        /* 
         * Работают напрямую с репозиторем. Разделение на команд чтения записи не делал
         */        
        public override async Task<EmployeeReply> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            var mode = Mapping(request);

            await employeeRepository.CreateAsync(mode);

            // TODO: Ответ
            return new EmployeeReply();

            EmployeeModel Mapping(CreateEmployeeRequest request)
            {
                return new EmployeeModel()
                {
                    FirstName = request.FirstName,
                    LastName  = request.LastName,
                };
            }
        }
        public override async Task<EmployeeReply> UpdateEmployee(UpdateEmployeeRequest request, ServerCallContext context)
        {
            var mode = Mapping(request);

            await employeeRepository.CreateAsync(mode);

            // TODO: Ответ
            return new EmployeeReply();

            EmployeeModel Mapping(UpdateEmployeeRequest request)
            {
                //TOOD: Должна находить в базе данных по ID обновлять свойства и сохранять если это через орм!
                return new EmployeeModel()
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                };
            }
        }
        public override async Task<EmployeeReply> DeleteEmployee(DeleteEmployeeRequest request, ServerCallContext context)
        {
            var mode = Mapping(request);

            await employeeRepository.CreateAsync(mode);

            // TODO: Ответ
            return new EmployeeReply();

            EmployeeModel Mapping(DeleteEmployeeRequest request)
            {
                //TOOD: Должна находить в базе данных по ID обновлять свойства и сохранять если это через орм!
                return new EmployeeModel()
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                };
            }
        }

        /* 
         * 
         */
        public override async Task GetWorkerStream(EmptyMessage request, IServerStreamWriter<WorkerAction> responseStream, ServerCallContext context)
        {
            // Возвращает всех 
            foreach (WorkerMessage message in EmployeeWorker.items.Values.ToArray())
            {
                await responseStream.WriteAsync(new WorkerAction { ActionType = Action.Default, Worker = message });
            }
        }


        
       
        




        private EmployeeReply Map(EmployeeModel model)
        {
            return new EmployeeReply()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                HaveChildren = model.HaveChildren,
                Sex = Map(model.Sex),

            };
        }
        private Sex Map(EmployeeSex model)
        {
            switch (model)
            {
                case EmployeeSex.Male:
                    return Sex.Male;
                case EmployeeSex.Female:
                    return Sex.Female;
                default:
                    return Sex.Default;
            }
        }
    }


    public static class EmployeeWorker
    {
        public static ConcurrentDictionary<int, WorkerMessage> items = new ConcurrentDictionary<int, WorkerMessage>();

        static EmployeeWorker()
        {
            for (int i = 0; i < 5000; i++)
            {
                items.TryAdd(i, new WorkerMessage()
                {
                    FirstName    = $"{i}",
                    LastName     = $"{i}",
                    MiddleName   = $"{i}",
                    Birthday     = 1,
                    HaveChildren = false,
                    Sex          = Sex.Male
                });
            }
        }
    }



}
