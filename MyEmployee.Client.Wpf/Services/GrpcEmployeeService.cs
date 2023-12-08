using GrpcEmployeeClient = MyEmployee.API.WorkerIntegration.WorkerIntegrationClient;

namespace MyEmployee.Client.Wpf.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class GrpcEmployeeService
    {
        private readonly GrpcEmployeeClient employeeClient;

        public GrpcEmployeeService(GrpcEmployeeClient employeeClient)
        {
            this.employeeClient = employeeClient;
        }
        
        public async Task GetAll(CancellationToken cancellationToken = default)
        {
            // посылаем пустое сообщение
            var responseData =  employeeClient.GetWorkerStream(new(), cancellationToken: cancellationToken);

            // получаем поток сервера
            var responseStream = responseData.ResponseStream;

            // извлекаем сообщение из потока
            while (await responseStream.MoveNext(cancellationToken))
            {
                var response = responseStream.Current;
                if (response == null)
                {
                    // TODO: Проверка на Null
                }

                // TODO: Обработать!
            }
        }
    }
}
