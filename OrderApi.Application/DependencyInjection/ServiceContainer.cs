using ecommerce.sharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;


namespace OrderApi.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IOrderServices, OrderService>(options =>
            {
                      options.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                    options.Timeout = TimeSpan.FromSeconds(1);

            });
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),//Specifies which exceptions should trigger a retry.
                BackoffType =DelayBackoffType.Constant,// Keeps the delay between retries fixed (constant)
                UseJitter =true,//Adds a small random variation (jitter) to the delay
                MaxRetryAttempts =3,
                Delay=TimeSpan.FromMilliseconds(500),
                OnRetry=args=>
                {
                    string message=$"OnRetry,Attempt :{args.AttemptNumber} Outcome{args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;

                }
            };
            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);

            });
            return services;
        }
    }
}
