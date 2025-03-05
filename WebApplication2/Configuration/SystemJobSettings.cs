using Microsoft.Extensions.Internal;
using WebApplication2.Configuration;

public class SystemJobSettings:ISystemJobSettings
{
    public int chunkSize { get; set; }
}