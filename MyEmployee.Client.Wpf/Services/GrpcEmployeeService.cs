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

                    yield return new EmployeeEvent();
                }
            }
        }
        
        /// <inheritdoc/>>  
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

        /// <inheritdoc/>>  
        public async Task CreateEmployee(EmployeeModel model)
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


            CreateEmployeeRequest MappingToCreateRequest(EmployeeModel model)
            {
                return new CreateEmployeeRequest()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
            }
        }

        /// <inheritdoc/>>   
        public async Task UpdateEmployee(EmployeeModel model)
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

            UpdateEmployeeRequest MappingToUpdateRequest(EmployeeModel model)
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
        public async Task DeleteEmployee(EmployeeModel model)
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

            DeleteEmployeeRequest MappingToDeleteRequest(EmployeeModel model)
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
