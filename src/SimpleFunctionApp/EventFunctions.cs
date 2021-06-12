using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SimpleFunctionApp
{
    public class EventFunctions
    {
        private IConfiguration Configuration { get; }

        public EventFunctions(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Function(nameof(QueueTrigger))]
        [QueueOutput("zero-queue")] 
        public async Task<string> QueueTrigger([QueueTrigger("zero-queue")] string myQueueItem)
        {
            var executionId = Guid.NewGuid();
            await using (var connection = new SqlConnection(Configuration["dbConnectionString"]))
            {
                await connection.ExecuteAsync(@"INSERT INTO Events Values (@executionId, @message, 1, NULL)",
                                             new { executionId = executionId, message= myQueueItem });

                var ms=new Random(DateTime.UtcNow.Millisecond).Next(5000, 15000);
                await Task.Delay(ms);

                await connection.ExecuteAsync(@"UPDATE Events SET Finished=1 WHERE ExecutionId= @executionId",
                    new { executionId = executionId });
            }

            var newMessage = GetNewMessage(myQueueItem);
            return newMessage;
        }

        private static string GetNewMessage(string myQueueItem)
        {
            var messageParts = myQueueItem.Split("_");
            var number = int.Parse(messageParts[1]);
            var newMessage = $"{messageParts[0]}_{number + 1}";
            return newMessage;
        }
    }
}