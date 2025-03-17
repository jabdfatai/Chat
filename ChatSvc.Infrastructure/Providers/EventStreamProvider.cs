using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserIdentitySvc.Core.Interface;

namespace SecureCommSvc.Infrastructure.Providers
{
 
    public class EventStreamProvider : IEventStreamProvider
    {

        ILogger<EventStreamProvider> logger;
        public EventStreamProvider(ILogger<EventStreamProvider> logger)
        {
            this.logger = logger;
        }
        public async Task<bool> SendOrderRequest(string topic, string message, string serveraddress)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = serveraddress,
                ClientId = Dns.GetHostName()

            };

            try
            {
                logger.LogInformation(message);
                using (var producer = new ProducerBuilder
                <Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync
                    (topic, new Message<Null, string>
                    {
                        Value = message
                    });

                    logger.LogInformation(result.ToString() + $"Delivery Timestamp:{result.Timestamp.UtcDateTime}");

                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());

            }

            return await Task.FromResult(false);
        }


    }
}
