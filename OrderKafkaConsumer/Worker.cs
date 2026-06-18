using Confluent.Kafka;

namespace OrderKafkaConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "order-processing-group", // קבוצת צרכנים (Consumer Group)
                AutoOffsetReset = AutoOffsetReset.Earliest // אם זו פעם ראשונה, תתחיל לקרוא מההתחלה
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("new-orders-topic"); // חייב להיות זהה ל-Topic שהגדרנו באפליקציה הראשית

            _logger.LogInformation("Consumer is waiting for new orders...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // עצירת השרשור עד שתגיע הודעה חדשה
                    var consumeResult = consumer.Consume(stoppingToken);

                    // הדפסת ההודעה ללוג
                    _logger.LogInformation($"[NEW ORDER RECEIVED] Offset: {consumeResult.Offset}");
                    _logger.LogInformation($"Details: {consumeResult.Message.Value}");

                    // כאן אפשר להוסיף לוגיקה עסקית אמיתית - כמו שליחת מייל חשבונית ללקוח
                }
            }
            catch (OperationCanceledException)
            {
                // קורה כשמכבים את השרת
                consumer.Close();
            }
        }
    }
}