namespace Alfavox.Models;

public class FilmProperties : BaseProperties
{
    public List<string> starships { get; set; }
    public List<string> vehicles { get; set; }
    public List<string> planets { get; set; }
    public string producer { get; set; }
    public string title { get; set; }
    public int episode_id { get; set; }
    public string director { get; set; }
    public string release_date { get; set; }
    public string opening_crawl { get; set; }
    public List<string> characters { get; set; }
    public List<string> species { get; set; }
}