using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using SimCode.Services.EmailApi.Models.Dto;
using System.Text;

namespace SimCode.Services.EmailApi.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionStrings;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailCartProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            serviceBusConnectionStrings = _configuration.GetValue<string>("ServiceBusConnectionStrings");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionStrings);

            _emailCartProcessor = client.CreateProcessor(emailCartQueue);

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecieved;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestRecieved(ProcessMessageEventArgs args)
        {
            //This is where you receive the message from the queue
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                //Log to the database
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

     
    }
}
