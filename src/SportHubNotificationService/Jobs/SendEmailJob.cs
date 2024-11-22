﻿using Hangfire;
using SportHubNotificationService.Domain.Models;
using SportHubNotificationService.Infrastructure.Services;

namespace SportHubNotificationService.Jobs;

public class SendEmailJob(
    MailSenderService service,
    ILogger<SendEmailJob> logger)
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 10, 15])]
    public async Task Execute(IEnumerable<string> recievers, string subject, string body)
    {
        try
        {
            var mailData = new MailData(recievers, subject, body);

            await service.Send(mailData);
        }
        catch (Exception ex)
        {
            logger.LogError("Cannot send email, ex: {ex}", ex.Message);
        }
    }
}