namespace Application.Abstractions;

public interface IEmailService
{
    Task SendAsync(string email, string subject, string message);
}
