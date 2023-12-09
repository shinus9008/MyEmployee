using Google.Protobuf;
using Grpc.Core;
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;

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

        // TODO: Авторизаййия и аутентификация не настроена

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
        public override async Task GetWorkerStream(
            IAsyncStreamReader<EmptyMessage>  requestStream, 
            IServerStreamWriter<WorkerAction> responseStream, 
            ServerCallContext context)
        {
            bool isReg = false;
            

            // считываем входящие сообщения в фоновой задаче
            // если поток завершился, то клиент завершил соединение
            await foreach (EmptyMessage message in requestStream.ReadAllAsync())
            {
                // Принемаем задачи на регистрацию или 
                if (isReg == false)
                {
                    // Регистрируем клиента!
                }
            }

            if(isReg)
            {
                // Снимаем регистрацию
            }
            






            foreach (var message in messages)
            {
                // Посылаем ответ, пока клиент не закроет поток
                if (!readTask.IsCompleted)
                {
                    await responseStream.WriteAsync(new Response { Content = message });
                    Console.WriteLine(message);
                    await Task.Delay(2000);
                }
            }

            return Task.CompletedTask;
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
}
