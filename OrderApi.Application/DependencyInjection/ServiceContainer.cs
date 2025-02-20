﻿using ecommerce.sharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
using Polly;
using Polly.Retry;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                 ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),

                BackoffType =DelayBackoffType.Constant,
                UseJitter=true, MaxRetryAttempts=3,Delay=TimeSpan.FromMilliseconds(500),OnRetry=args=>
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
