using Grpc.Core;
using Microsoft.Extensions.Logging;
using MyEmployee.API.Models;
using MyEmployee.Domain.AggregateModels.EmployeeAggregates;
using MyEmployee.Shared;
using System.Reactive.Linq;

namespace MyEmployee.API.Gprc
{
    public class WorkerIntegrationService : WorkerIntegration.WorkerIntegrationBase
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IEmployeeEventObservable employeeEvent;
        private readonly ILogger<WorkerIntegrationService> logger;

        public WorkerIntegrationService(
            IEmployeeRepository employeeRepository,
            IEmployeeEventObservable employeeEvent, ILogger<WorkerIntegrationService> logger)
        {
            this.employeeRepository = employeeRepository;
            this.employeeEvent = employeeEvent;
            this.logger = logger;
        }

        // TODO: Авторизаййия и аутентификация не настроена

        /* 
         * Работают напрямую с репозиторем. Разделение на команд чтения записи не делал
         */
        public override async Task GetEmployeeStream(EmptyMessage request, IServerStreamWriter<EmployeeReply> responseStream, ServerCallContext context)
        {
            await foreach (var employee in employeeRepository.GetAllAsync())
            {
                var dto = Map(employee);

                await responseStream.WriteAsync(dto);
            }
        }
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
                    LastName = request.LastName,
                };
            }
        }
        public override async Task<EmployeeReply> UpdateEmployee(UpdateEmployeeRequest request, ServerCallContext context)
        {
            var mode = Mapping(request);

            await employeeRepository.UpdateAsync(mode);

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

            await employeeRepository.DeleteAsync(mode);

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
         * Подписака на изменения
         */
        public override async Task GetWorkerStream(
            IAsyncStreamReader<EmptyMessage> requestStream,
            IServerStreamWriter<WorkerAction> responseStream,
            ServerCallContext context)
        {
            logger.LogInformation($"{nameof(GetWorkerStream)} - Start");

            // Фоновая задача приема сообщений
            var readTask = Task.Run(async () =>
            {
                logger.LogInformation($"{nameof(GetWorkerStream)} - AsyncStreamReader опрос");

                // считываем входящие сообщения в фоновой задаче
                // если поток завершился, то клиент завершил соединение
                await foreach (EmptyMessage message in requestStream.ReadAllAsync())
                {
                    logger.LogDebug($"{nameof(GetWorkerStream)} - AsyncStreamReader принято сообщение");
                }

                logger.LogInformation($"{nameof(GetWorkerStream)} - AsyncStreamReader законяился");

            });

            
            logger.LogInformation($"{nameof(GetWorkerStream)} - Подписываемся на события");

            // Подписываемся на события изменения заказчиков
            // и передаем в выходной поток
            using (employeeEvent.Observable
                .Select(ev => Observable.FromAsync(async () =>
                {
                    var dto = Maping(ev);
                    if(!readTask.IsCompleted)
                    {
                        logger.LogDebug($"{nameof(GetWorkerStream)} - ServerStreamWriter - передача события");
                        await responseStream.WriteAsync(dto);
                    }
                   
                }))
                .Merge(10)
                .Subscribe())
            {
                logger.LogInformation($"{nameof(GetWorkerStream)} - Одидаем завершение AsyncStreamReader");

                // Одидаем завершение потока чтения
                await readTask;
            }

            logger.LogInformation($"{nameof(GetWorkerStream)} - End");
        }

        //TODO: Использовать маппер
        #region Maping

        static WorkerAction Maping(EmployeeEvent model)
        {
            return new WorkerAction()
            {
                //TODO: Напиать маппинг
                ActionType = Maping(model.Action),
                Worker     = Maping(model.Employee),
            };
        }

        static Action Maping(EmployeeEventType model)
        {
            switch (model)
            {
                case EmployeeEventType.Create: return Action.Create;                  
                case EmployeeEventType.Update: return Action.Update;                  
                case EmployeeEventType.Delete: return Action.Delete;                    
                default: return Action.Default;  
            }
        }

        static EmployeeReply Maping(EmployeePoco model)
        {
            return new EmployeeReply()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
        }
        private EmployeeReply Map(EmployeeModel model)
        {
            return new EmployeeReply()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,

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

        #endregion
    }
}
