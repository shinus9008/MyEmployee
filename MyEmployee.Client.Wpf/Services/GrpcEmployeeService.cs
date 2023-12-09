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
    public class GrpcEmployeeService : IEmployeeService, IDisposable
    {
        private readonly GrpcChannel channel;
        private readonly GrpcEmployeeClient client;

        public GrpcEmployeeService()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7074");
            this.client = new GrpcEmployeeClient(channel);
        }

        public void Dispose()
        {
            channel.Dispose();
        }

        //TODO: Почитать о EnumeratorCancellation
        public async IAsyncEnumerable<EmployeeModel> GetAllEmployes([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // посылаем пустое сообщение 
            using (var call = client.GetEmployeeStream(new(), cancellationToken: cancellationToken))
            {
                // получаем поток данных
                var responseStream = call.ResponseStream;

                // Читаем поток
                while (await responseStream.MoveNext(cancellationToken))
                {
                    var response = responseStream.Current;
                    if (response == null)
                    {
                        // TODO: Проверка на Null
                        continue;
                    }

                    var result = Mapping(response);

                    yield return result;
                }
            }
        }
        public async IAsyncEnumerable<EmployeeModel> GetAllEvents([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            yield break;
        }







        private EmployeeModel Mapping(EmployeeReply response)
        {
            return new EmployeeModel()
            {
                //TODO: Маппинг
                Id          = response.Id,
                FirstName   = response.FirstName,
                LastName    = response.LastName,
            };
        }
        private EmployeeModel Mapping(WorkerAction response)
        {
            return new EmployeeModel()
            {
                //TODO: Маппинг
                Id = int.Parse(response.Worker.FirstName)
            };
        }
    }
}
