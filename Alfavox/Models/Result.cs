namespace Alfavox.Models;

public class Result<T> where T : BaseProperties
{
    public T properties { get; set; }
    public string _id { get; set; }
    public string description { get; set; }
    public string uid { get; set; }
    public int __v { get; set; }
}