using Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string email, string subject, string message)
    {
        _logger.LogInformation($"Message: {message} has been send to Email: {email} with subject: {subject}");
        return Task.CompletedTask;
    }
}
