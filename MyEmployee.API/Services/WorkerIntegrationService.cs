using Grpc.Core;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MyEmployee.API.Services
{
    public class WorkerIntegrationService: WorkerIntegration.WorkerIntegrationBase
    {
        IObservable<WorkerAction> observable = new Subject<WorkerAction>();

        //public override async Task SetWorkerStream(
        //    IAsyncStreamReader <EmptyMessage> requestStream, 
        //    IServerStreamWriter<WorkerAction> responseStream, 
        //    ServerCallContext context)
        //{
        //    // Клиент подключился
        //    // Пока клиент не завершит передачу подключение сохраняется
        //    await foreach (var message in requestStream.ReadAllAsync()) 
        //    { 

        //    }
        //}

        public override Task GetEmployeeList(EmployeeListRequest request, IServerStreamWriter<EmployeeListReply> responseStream, ServerCallContext context)
        {
            return base.GetEmployeeList(request, responseStream, context);
        }
        public override Task GetEmployeeStream(EmptyMessage request, IServerStreamWriter<EmployeeReply> responseStream, ServerCallContext context)
        {
            return base.GetEmployeeStream(request, responseStream, context);
        }
        public override Task<EmployeeReply> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            return base.CreateEmployee(request, context);
        }
        public override Task<EmployeeReply> UpdateEmployee(UpdateEmployeeRequest request, ServerCallContext context)
        {
            return base.UpdateEmployee(request, context);
        }
        public override Task<EmployeeReply> DeleteEmployee(UpdateEmployeeRequest request, ServerCallContext context)
        {
            return base.DeleteEmployee(request, context);
        }
        public override async Task GetWorkerStream(EmptyMessage request, IServerStreamWriter<WorkerAction> responseStream, ServerCallContext context)
        {
            // Возвращает всех 
            foreach (WorkerMessage message in EmployeeWorker.items.Values.ToArray())
            {
                await responseStream.WriteAsync(new WorkerAction { ActionType = Action.Default, Worker = message });
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
