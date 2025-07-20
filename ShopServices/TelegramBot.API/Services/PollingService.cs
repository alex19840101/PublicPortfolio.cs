using System;
using Microsoft.Extensions.Logging;

namespace TelegramBot.API.Services;

/// <summary> Сервис для опроса Telegram Bot API </summary>
/// <param name="serviceProvider"></param>
/// <param name="logger"></param>
public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<TelegramUpdatesReceiverService>(serviceProvider, logger);