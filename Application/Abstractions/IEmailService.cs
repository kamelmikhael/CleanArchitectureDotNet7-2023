namespace Application.Abstractions;

public interface IEmailService
{
    Task Send(string email, string subject, string message);
}
