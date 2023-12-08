using Grpc.Net.Client;
using MyEmployee.API;
using MyEmployee.Client.Wpf.Models;
using System.Runtime.CompilerServices;
using GrpcEmployeeClient = MyEmployee.API.WorkerIntegration.WorkerIntegrationClient;

namespace MyEmployee.Client.Wpf.Services
{
    /// <summary>
    /// Зaгружает данные через Grpc клиент
    /// </summary>
    public class GrpcEmployeeService : IEmployeeService
    {
        private readonly GrpcEmployeeClient employeeClient;

        public GrpcEmployeeService()
        {
            this.employeeClient = employeeClient;
        }

        //TODO: Почитать о EnumeratorCancellation
        public async IAsyncEnumerable<EmployeeModel> GetAllEmployes([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using (var channel = GrpcChannel.ForAddress("https://localhost:7074"))
            {
                var employeeClient = new GrpcEmployeeClient(channel);

                // посылаем пустое сообщение
                var responseData = employeeClient.GetWorkerStream(new(), cancellationToken: cancellationToken);

                // получаем поток сервера
                var responseStream = responseData.ResponseStream;

                // извлекаем сообщение из потока
                while (await responseStream.MoveNext(cancellationToken))
                {
                    var response = responseStream.Current;
                    if (response == null)
                    {
                        // TODO: Проверка на Null
                        continue;
                    }

                    if (response.ActionType != API.Action.Default)
                    {
                        // TODO: Лог!
                        continue;
                    }


                    var result = Mapping(response);

                    yield return result;
                }
            }
        }

        private EmployeeModel Mapping(WorkerAction response)
        {
            return new EmployeeModel()
            {
                //TODO: Маппинг
                Key = 1
            };
        }
    }
}
