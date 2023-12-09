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

        //TODO: Почитать о EnumeratorCancellation
        public GrpcEmployeeService()
        {
            this.channel = GrpcChannel.ForAddress("https://localhost:7074");
            this.client = new GrpcEmployeeClient(channel);
        }

        /// <inheritdoc/>>  
        public async IAsyncEnumerable<EmployeeEvent> GetAllEvents([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // получаем объект AsyncDuplexStreamingCall 
            using (var call = client.GetWorkerStream(cancellationToken: cancellationToken))
            {
                try
                {
                    //
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // Отправляем сообщение
                        // Сервер должен подписать нас на получение событий
                        await call.RequestStream.WriteAsync(new EmptyMessage());

                        // Обратный поток не должен прерываться!
                        // Если он пуст (значит чсервер не успел нас зарегистрирвать?)
                        while (await call.ResponseStream.MoveNext(cancellationToken))
                        {
                            var response = call.ResponseStream.Current;
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
                finally
                {
                    // завершаем чтение сообщений
                    await call.RequestStream.CompleteAsync();
                }
                // Отправляем сообщение
                // Сервер должен подписать нас на получение событий
                await call.RequestStream.WriteAsync(new EmptyMessage());      
            }
        }
        
        /// <inheritdoc/>>  
        public async IAsyncEnumerable<EmployeePoco> GetAllEmployes([EnumeratorCancellation] CancellationToken cancellationToken)
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

        /// <inheritdoc/>>  
        public async Task CreateEmployee(EmployeePoco model)
        {
            try
            {
                var request = MappingToCreateRequest(model);

                var response = await client.CreateEmployeeAsync(request).ConfigureAwait(false);

                if (response != null)
                {
                    //TODO: Тут ожно проверить результат операции и сгенерировать исключение согласно тербованю интерфейса
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка!",ex);
            }


            CreateEmployeeRequest MappingToCreateRequest(EmployeePoco model)
            {
                return new CreateEmployeeRequest()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
            }
        }

        /// <inheritdoc/>>   
        public async Task UpdateEmployee(EmployeePoco model)
        {
            try
            {
                var request = MappingToUpdateRequest(model);

                var response = await client.UpdateEmployeeAsync(request).ConfigureAwait(false);

                if (response != null)
                {
                    //TODO: Тут ожно проверить результат операции и сгенерировать исключение согласно тербованю интерфейса
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка!", ex);
            }

            UpdateEmployeeRequest MappingToUpdateRequest(EmployeePoco model)
            {
                return new UpdateEmployeeRequest()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
            }
        }

        /// <inheritdoc/>>        
        public async Task DeleteEmployee(EmployeePoco model)
        {
            try
            {
                var request = MappingToDeleteRequest(model);

                var response = await client.DeleteEmployeeAsync(request).ConfigureAwait(false);

                if (response != null)
                {
                    //TODO: Тут ожно проверить результат операции и сгенерировать исключение согласно тербованю интерфейса
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка!", ex);
            }

            DeleteEmployeeRequest MappingToDeleteRequest(EmployeePoco model)
            {
                return new DeleteEmployeeRequest()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
            }
        }

        /// <inheritdoc/>> 
        public void Dispose()
        {
            channel.Dispose();
        }

        private EmployeePoco Mapping(EmployeeReply response)
        {
            return new EmployeePoco()
            {
                //TODO: Маппинг
                Id          = response.Id,
                FirstName   = response.FirstName,
                LastName    = response.LastName,
            };
        }             

        private EmployeeEvent Mapping(WorkerAction response)
        {
            return new EmployeeEvent(
                Mapping(response.Worker),
                MappingAction(response.ActionType));

            EmployeeEventAction MappingAction(API.Action response)
            {
                switch (response)
                {
                    case API.Action.Delete:
                        return EmployeeEventAction.Delete;
                    case API.Action.Update:
                    case API.Action.Create:
                    default:
                        return EmployeeEventAction.UpdateOrCreate;
                }
            }
        }



    }
}
