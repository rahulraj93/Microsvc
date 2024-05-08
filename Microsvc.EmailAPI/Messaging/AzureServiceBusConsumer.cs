using Azure.Messaging.ServiceBus;
using Microsvc.Services.EmailAPI.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace Microsvc.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailCartProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            this._configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueue:EmailCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += _emailCartProcessor_ProcessMessageAsync;
            _emailCartProcessor.ProcessErrorAsync += _emailCartProcessor_ProcessErrorAsync;
            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }

        private Task _emailCartProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task _emailCartProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto cartMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
