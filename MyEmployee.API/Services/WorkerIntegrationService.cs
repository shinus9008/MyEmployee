using Grpc.Core;
using MyEmployee.API;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography.X509Certificates;

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

        public override async Task GetWorkerStream(
            EmptyMessage request, 
            IServerStreamWriter<WorkerAction> responseStream, ServerCallContext context)
        {
            // TODO: Аутентификация

            // Потоковая передача на стороне сервера

            try
            {
                // Возвращает всех 
                foreach (WorkerMessage message in EmployeeWorker.items.Values.ToArray()) 
                {
                    await responseStream.WriteAsync(new WorkerAction { ActionType = Action.Default, Worker = message });
                }

            }
            catch (Exception ex)
            {
                //TODO: Логер!
            }

            // Потоковая передача завершается



            // Сохраняем его как подписчика

            // Слушаем сообщения клиента

            // Если сообщения закончились

            // Отписываемся 
           



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
