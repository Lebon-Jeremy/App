using System;
using Microsoft.AspNetCore.SignalR;
using Webscraper.Src.Consumers;
using Webscraper.Src.Producers;

namespace Webscraper;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHubContext<MyHub> _hubContext; // Injected SignalR hub context

    public Worker(ILogger<Worker> logger, IHubContext<MyHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;

    }

    private async Task SendDataToClientsAsync(string data)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveData", data); // "ReceiveData" is the name of the client-side method
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        await SendDataToClientsAsync("newData")
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            CryptoProducer1 cryptoProducer1 = new();

            var rawjson = await cryptoProducer1.GetCurrentValueAsync(ACCEPTED_COIN.BTC);

            if (rawjson != null)
            {
                CryptoConsumer1 cryptoConsumer1 = new CryptoConsumer1(rawjson);
                ExchangeRate? test2 = cryptoConsumer1.FormatItem();
                var running = cryptoConsumer1.WriteToFile();

                if (test2 != null)
                {
                    _logger.LogInformation("Actual BTC value {test}", test2.Rate);
                }
                else
                {
                    _logger.LogInformation("CryptoConsumer1 Value is Null");
                }

             
            }
            else
            {
                _logger.LogInformation("CryptoProducer1 Value is Null");
            }
            await Task.Delay(1000, stoppingToken);
            return;
        }
    }
}

