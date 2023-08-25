namespace Api.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set;}
    public bool EnabledDetailedErrors { get; set; }
    public bool EnabledSensitiveDataLogging { get; set; }
}
