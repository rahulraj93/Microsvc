using Azure.Messaging.ServiceBus;
using Microsvc.Services.EmailAPI.Models.Dto;
using Microsvc.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Microsvc.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly string _registrationQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private ServiceBusProcessor _emailCartProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration
                                        ,EmailService emailService)
        {
            this._configuration = configuration;
            this._emailService = emailService;
            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueue:EmailCartQueue");
            _registrationQueue = _configuration.GetValue<string>("TopicAndQueue:EmailCartQueue");

            //var client = new ServiceBusClient(_serviceBusConnectionString);

            //_emailCartProcessor = client.CreateProcessor(_emailCartQueue);
        }

        public async Task Start()
        {
            //_emailCartProcessor.ProcessMessageAsync += _emailCartProcessor_ProcessMessageAsync;
            //_emailCartProcessor.ProcessErrorAsync += _emailCartProcessor_ProcessErrorAsync;


            //_emailCartProcessor.ProcessMessageAsync += _registrationProcessor_ProcessMessageAsync;
            //_emailCartProcessor.ProcessErrorAsync += _emailCartProcessor_ProcessErrorAsync;

            //await _emailCartProcessor.StartProcessingAsync();
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
                await _emailService.EmailCartAndLog(cartMessage);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex) {
                throw;
            }
        }

        private async Task _registrationProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string registrationMessage = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailAndLog(registrationMessage);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
