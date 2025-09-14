namespace Alfavox.Models;

public class Root<T> where T : BaseProperties
{
    public string message { get; set; }
    public Result<T> result { get; set; }
    public string apiVersion { get; set; }
    public DateTime timestamp { get; set; }
    public Support support { get; set; }
    public Social social { get; set; }
}