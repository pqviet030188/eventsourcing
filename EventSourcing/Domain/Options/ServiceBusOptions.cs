namespace Demo.Models.Options;

public class ServiceBusOptions
{
    public string PropertyEventConnectionString { get; set; } = null!;
    public string PropertyEventTopic { get; set; } = null!;
}
