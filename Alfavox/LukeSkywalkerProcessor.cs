using System.Text.Json;
using Alfavox.Models;

namespace Alfavox;

public class LukeSkywalkerProcessor(
    ILogger<LukeSkywalkerProcessor> logger,
    IConfiguration configuration,
    HttpClient httpClient)
{
    private const string SwApiUrlConfigEntry = "LukeSkywalkerSwApiEndpoint";
    private const string LukeSkywalkerOutputFileConfigEntry = "LukeSkywalkerOutputFile";

    public async Task<string> GetLukeSkywalkerInfo()
    {
        try
        {
            var apiUrl = configuration.GetSection(SwApiUrlConfigEntry).Value ?? string.Empty;
            var uri = new Uri(apiUrl);

            logger.LogInformation("Attempting to query Luke Skywalker's info from Star Wars API using URL: {URL}",
                apiUrl);

            var lukeInfo = Task.Run(() => httpClient.GetAsync(uri)).Result;

            var lukeInfoContent = await lukeInfo.Content.ReadAsStringAsync();

            if (!lukeInfo.IsSuccessStatusCode)
            {
                var errorMessage =
                    $"Failed to query Luke Skywalker info from Star Wars API, status code: {lukeInfo.StatusCode}";
                logger.LogError(errorMessage);
                return errorMessage;
            }

            var lukeInfoDeserialized =
                JsonSerializer.Deserialize<Root<PeopleProperties>>(lukeInfoContent);

            if (lukeInfoDeserialized == null)
            {
                var errorMessage = $"Error while deserializing returned info content: {lukeInfoContent}";
                logger.LogError(errorMessage);
                return errorMessage;
            }

            logger.LogInformation("Querying more detailed, underlying info about Luke Skywalker");
            var filmTitles = await GetFilmsTitles(lukeInfoDeserialized.result.properties.films);
            if (filmTitles.Count != 0)
                lukeInfoDeserialized.result.properties.films = filmTitles;

            var vehicleNames = await GetVehicleNames(lukeInfoDeserialized.result.properties.vehicles);
            if (vehicleNames.Count != 0)
                lukeInfoDeserialized.result.properties.vehicles = vehicleNames;

            var starshipNames = await GetStarshipNames(lukeInfoDeserialized.result.properties.starships);
            if (starshipNames.Count != 0)
                lukeInfoDeserialized.result.properties.starships = starshipNames;

            var lukeInfoSerialized = JsonSerializer.Serialize(lukeInfoDeserialized, new JsonSerializerOptions() { WriteIndented = true });
            var lukeSkywalkerOutputFile =
                configuration.GetSection(LukeSkywalkerOutputFileConfigEntry).Value ?? string.Empty;

            logger.LogInformation("Writing Luke Skywalker info to file: {File}", lukeSkywalkerOutputFile);
            Task.Run(() => File.WriteAllTextAsync(lukeSkywalkerOutputFile, lukeInfoSerialized)).Wait();

            return lukeInfoSerialized;
        }
        catch (Exception ex)
        {
            var exceptionMessage = $"Error while processing Star Wars API data, Exception: {ex.Message}";
            logger.LogCritical(exceptionMessage);
            return exceptionMessage;
        }
    }


    private async Task<List<string>> GetFilmsTitles(List<string> filmsUrls)
    {
        logger.LogInformation("Querying detailed info about Films");
        var filmsInfo = await QueryUrls<FilmProperties>(filmsUrls);

        if (filmsInfo.Count == 0)
        {
            logger.LogError("No Films found");
            return [];
        }

        return filmsInfo.Select(film => film.result.properties.title).ToList();
    }

    private async Task<List<string>> GetVehicleNames(List<string> vehiclesUrls)
    {
        logger.LogInformation("Querying detailed info about Vehicles");
        var vehicleInfo = await QueryUrls<VehicleProperties>(vehiclesUrls);

        if (vehicleInfo.Count == 0)
        {
            logger.LogError("No Vehicles found");
            return [];
        }

        return vehicleInfo.Select(film => film.result.properties.name).ToList();
    }

    private async Task<List<string>> GetStarshipNames(List<string> starshipsUrls)
    {
        logger.LogInformation("Querying detailed info about Starships");
        var starshipInfo = await QueryUrls<VehicleProperties>(starshipsUrls);

        if (starshipInfo.Count == 0)
        {
            logger.LogError("No Starships found");
            return [];
        }

        return starshipInfo.Select(film => film.result.properties.name).ToList();
    }

    private async Task<List<Root<T>>> QueryUrls<T>(List<string> urls) where T : BaseProperties
    {
        List<Root<T>> deserializedItems = [];
        foreach (var url in urls)
        {
            logger.LogInformation("Querying URL {Url}", url);
            var item = Task.Run(() => httpClient.GetAsync(url)).Result;


            if (!item.IsSuccessStatusCode)
            {
                logger.LogError("Failed to query URL {Url}, Status Code: {StatusCode}", url, item.StatusCode);
                continue;
            }

            var itemContent = await item.Content.ReadAsStringAsync();
            var itemDeserialized = JsonSerializer.Deserialize<Root<T>>(await item.Content.ReadAsStringAsync());

            if (itemDeserialized == null)
            {
                logger.LogError("Error while deserializing returned info content: {Content}", itemContent);
                continue;
            }

            deserializedItems.Add(itemDeserialized);
        }

        return deserializedItems;
    }
}