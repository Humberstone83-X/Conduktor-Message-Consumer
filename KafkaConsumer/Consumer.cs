using System;
using Confluent.Kafka;
using System.Text.Json;
using ThreeDSKafkaProxy;
using System.Text;
using ThreeDSKafkaProxy.Models;

namespace KafkaConsumer
{
    class Consumer
    {
        public Consumer()
        {
        }

        public async Task ConsumeMessages()
        {
            string kafkaBootstrapServers = "localhost:9092";
            string kafkaTopic = "onboarding_merchant_created";
            string kafkaConsumerGroup = "test-consumer-group";

            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaBootstrapServers,
                GroupId = kafkaConsumerGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(kafkaTopic);

                Console.WriteLine($"Consumer is now listening for messages on topic '{kafkaTopic}'...");

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume();

                            Console.WriteLine($"Received message: {consumeResult.Message.Value}");

                            // Invoke Lambda function with message payload
                            var lambdaContext = new TestLambdaContext();
                            var jsonData = JsonSerializer.Serialize(
                            new Event()
                            {
                                records =
                                new Record() {
                                    Value = new List<MerchantOnboardingEvent>() {
                                            new()
                                            {
                                                Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(consumeResult.Message.Key)),
                                                Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(consumeResult.Message.Value)),
                                                Offset = (int)consumeResult.Offset.Value,
                                            }
                                        }
                                    }
                            });
                            await new Function().FunctionHandlerAsync(JsonDocument.Parse(jsonData).RootElement, lambdaContext);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occurred: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Consumer was cancelled.");
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
    }

    public class Event
    {
        public object records { get; set; } = new();
    }

    public class Record
    {
        public List<MerchantOnboardingEvent> Value { get; set; } = new();
    }
}
