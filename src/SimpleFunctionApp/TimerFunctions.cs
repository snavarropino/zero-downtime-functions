using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SimpleFunctionApp
{
    public class TimerFunctions
    {
        const string RunEveryMinuteCron = "0 * * * * *";

        [Function(nameof(TimerTrigger))]
        public static void TimerTrigger([TimerTrigger(RunEveryMinuteCron)] TimerInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(TimerTrigger));

            logger.LogInformation("Doing stuff");

        }
    }
}