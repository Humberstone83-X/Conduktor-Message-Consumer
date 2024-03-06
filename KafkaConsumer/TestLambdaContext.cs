using Amazon.Lambda.Core;

namespace KafkaConsumer
{

    class TestLambdaContext : ILambdaContext
    {
        string ILambdaContext.AwsRequestId => Guid.NewGuid().ToString();

        IClientContext ILambdaContext.ClientContext => throw new NotImplementedException();

        string ILambdaContext.FunctionName => throw new NotImplementedException();

        string ILambdaContext.FunctionVersion => throw new NotImplementedException();

        ICognitoIdentity ILambdaContext.Identity => throw new NotImplementedException();

        string ILambdaContext.InvokedFunctionArn => throw new NotImplementedException();

        ILambdaLogger ILambdaContext.Logger => new Logger();

        string ILambdaContext.LogGroupName => throw new NotImplementedException();

        string ILambdaContext.LogStreamName => throw new NotImplementedException();

        int ILambdaContext.MemoryLimitInMB => throw new NotImplementedException();

        TimeSpan ILambdaContext.RemainingTime => throw new NotImplementedException();

        private class Logger : ILambdaLogger
        {
            public void Log(string message)
            {
                Console.WriteLine(message);
            }

            public void LogLine(string message)
            {
                Console.WriteLine(message);
            }

            public void LogLine(string format, params object[] args)
            {
                Console.WriteLine(format, args);
            }
        }
    }
}
