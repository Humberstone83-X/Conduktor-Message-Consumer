using KafkaConsumer;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Kafka consumer...");

        var kafkaConsumer = new Consumer();

        try
        {
            await kafkaConsumer.ConsumeMessages();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}