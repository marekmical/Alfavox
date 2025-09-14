using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace Alfavox.Tests;

public class Tests
{
    private const string apiUrl = "https://www.swapi.tech/api/people/1";
    private const string fileName = "test.json";
    private const string mediaType = "application/json";
    private const string expectedFilmTitle = "A New Hope";
    private const string expectedStarshipName = "Sand Crawler";
    private const string expectedVehicleName = "CR90 corvette";

    private const string LukeInfo =
        "{\n  \"message\": \"ok\",\n  \"result\": {\n    \"properties\": {\n      \"created\": \"2025-09-14T00:12:09.502Z\",\n      \"edited\": \"2025-09-14T00:12:09.502Z\",\n      \"name\": \"Luke Skywalker\",\n      \"gender\": \"male\",\n      \"skin_color\": \"fair\",\n      \"hair_color\": \"blond\",\n      \"height\": \"172\",\n      \"eye_color\": \"blue\",\n      \"mass\": \"77\",\n      \"homeworld\": \"https://www.swapi.tech/api/planets/1\",\n      \"birth_year\": \"19BBY\",\n      \"vehicles\": [\n        \"https://www.swapi.tech/api/vehicles/14\",\n        \"https://www.swapi.tech/api/vehicles/30\"\n      ],\n      \"starships\": [\n        \"https://www.swapi.tech/api/starships/12\",\n        \"https://www.swapi.tech/api/starships/22\"\n      ],\n      \"films\": [\n        \"https://www.swapi.tech/api/films/1\",\n        \"https://www.swapi.tech/api/films/2\",\n        \"https://www.swapi.tech/api/films/3\",\n        \"https://www.swapi.tech/api/films/6\"\n      ],\n      \"url\": \"https://www.swapi.tech/api/people/1\"\n    },\n    \"_id\": \"5f63a36eee9fd7000499be42\",\n    \"description\": \"A person within the Star Wars universe\",\n    \"uid\": \"1\",\n    \"__v\": 4\n  },\n  \"apiVersion\": \"1.0\",\n  \"timestamp\": \"2025-09-14T06:36:27.451Z\",\n  \"support\": {\n    \"contact\": \"admin@swapi.tech\",\n    \"donate\": \"https://www.paypal.com/donate/?business=2HGAUVTWGR5T2&no_recurring=0&item_name=Support+Swapi+and+keep+the+galaxy%27s+data+free%21+Your+donation+fuels+open-source+innovation+and+helps+us+grow.+Thank+you%21+%F0%9F%9A%80&currency_code=USD\",\n    \"partnerDiscounts\": {\n      \"saberMasters\": {\n        \"link\": \"https://www.swapi.tech/partner-discount/sabermasters-swapi\",\n        \"details\": \"Use this link to automatically get $10 off your purchase!\"\n      },\n      \"heartMath\": {\n        \"link\": \"https://www.heartmath.com/ryanc\",\n        \"details\": \"Looking for some Jedi-like inner peace? Take 10% off your heart-brain coherence tools from the HeartMath Institute!\"\n      }\n    }\n  },\n  \"social\": {\n    \"discord\": \"https://discord.gg/zWvA6GPeNG\",\n    \"reddit\": \"https://www.reddit.com/r/SwapiOfficial/\",\n    \"github\": \"https://github.com/semperry/swapi/blob/main/CONTRIBUTORS.md\"\n  }\n}";

    private const string FilmInfo =
        "{\n  \"message\": \"ok\",\n  \"result\": {\n    \"properties\": {\n      \"created\": \"2025-09-14T00:12:09.507Z\",\n      \"edited\": \"2025-09-14T00:12:09.507Z\",\n      \"starships\": [\n        \"https://www.swapi.tech/api/starships/2\",\n        \"https://www.swapi.tech/api/starships/3\",\n        \"https://www.swapi.tech/api/starships/5\",\n        \"https://www.swapi.tech/api/starships/9\",\n        \"https://www.swapi.tech/api/starships/10\",\n        \"https://www.swapi.tech/api/starships/11\",\n        \"https://www.swapi.tech/api/starships/12\",\n        \"https://www.swapi.tech/api/starships/13\"\n      ],\n      \"vehicles\": [\n        \"https://www.swapi.tech/api/vehicles/4\",\n        \"https://www.swapi.tech/api/vehicles/6\",\n        \"https://www.swapi.tech/api/vehicles/7\",\n        \"https://www.swapi.tech/api/vehicles/8\"\n      ],\n      \"planets\": [\n        \"https://www.swapi.tech/api/planets/1\",\n        \"https://www.swapi.tech/api/planets/2\",\n        \"https://www.swapi.tech/api/planets/3\"\n      ],\n      \"producer\": \"Gary Kurtz, Rick McCallum\",\n      \"title\": \"A New Hope\",\n      \"episode_id\": 4,\n      \"director\": \"George Lucas\",\n      \"release_date\": \"1977-05-25\",\n      \"opening_crawl\": \"It is a period of civil war.\\r\\nRebel spaceships, striking\\r\\nfrom a hidden base, have won\\r\\ntheir first victory against\\r\\nthe evil Galactic Empire.\\r\\n\\r\\nDuring the battle, Rebel\\r\\nspies managed to steal secret\\r\\nplans to the Empire's\\r\\nultimate weapon, the DEATH\\r\\nSTAR, an armored space\\r\\nstation with enough power\\r\\nto destroy an entire planet.\\r\\n\\r\\nPursued by the Empire's\\r\\nsinister agents, Princess\\r\\nLeia races home aboard her\\r\\nstarship, custodian of the\\r\\nstolen plans that can save her\\r\\npeople and restore\\r\\nfreedom to the galaxy....\",\n      \"characters\": [\n        \"https://www.swapi.tech/api/people/1\",\n        \"https://www.swapi.tech/api/people/2\",\n        \"https://www.swapi.tech/api/people/3\",\n        \"https://www.swapi.tech/api/people/4\",\n        \"https://www.swapi.tech/api/people/5\",\n        \"https://www.swapi.tech/api/people/6\",\n        \"https://www.swapi.tech/api/people/7\",\n        \"https://www.swapi.tech/api/people/8\",\n        \"https://www.swapi.tech/api/people/9\",\n        \"https://www.swapi.tech/api/people/10\",\n        \"https://www.swapi.tech/api/people/12\",\n        \"https://www.swapi.tech/api/people/13\",\n        \"https://www.swapi.tech/api/people/14\",\n        \"https://www.swapi.tech/api/people/15\",\n        \"https://www.swapi.tech/api/people/16\",\n        \"https://www.swapi.tech/api/people/18\",\n        \"https://www.swapi.tech/api/people/19\",\n        \"https://www.swapi.tech/api/people/81\"\n      ],\n      \"species\": [\n        \"https://www.swapi.tech/api/species/1\",\n        \"https://www.swapi.tech/api/species/2\",\n        \"https://www.swapi.tech/api/species/3\",\n        \"https://www.swapi.tech/api/species/4\",\n        \"https://www.swapi.tech/api/species/5\"\n      ],\n      \"url\": \"https://www.swapi.tech/api/films/1\"\n    },\n    \"_id\": \"5f63a117cf50d100047f9762\",\n    \"description\": \"A Star Wars Film\",\n    \"uid\": \"1\",\n    \"__v\": 2\n  },\n  \"apiVersion\": \"1.0\",\n  \"timestamp\": \"2025-09-14T06:42:41.825Z\",\n  \"support\": {\n    \"contact\": \"admin@swapi.tech\",\n    \"donate\": \"https://www.paypal.com/donate/?business=2HGAUVTWGR5T2&no_recurring=0&item_name=Support+Swapi+and+keep+the+galaxy%27s+data+free%21+Your+donation+fuels+open-source+innovation+and+helps+us+grow.+Thank+you%21+%F0%9F%9A%80&currency_code=USD\",\n    \"partnerDiscounts\": {\n      \"saberMasters\": {\n        \"link\": \"https://www.swapi.tech/partner-discount/sabermasters-swapi\",\n        \"details\": \"Use this link to automatically get $10 off your purchase!\"\n      },\n      \"heartMath\": {\n        \"link\": \"https://www.heartmath.com/ryanc\",\n        \"details\": \"Looking for some Jedi-like inner peace? Take 10% off your heart-brain coherence tools from the HeartMath Institute!\"\n      }\n    }\n  },\n  \"social\": {\n    \"discord\": \"https://discord.gg/zWvA6GPeNG\",\n    \"reddit\": \"https://www.reddit.com/r/SwapiOfficial/\",\n    \"github\": \"https://github.com/semperry/swapi/blob/main/CONTRIBUTORS.md\"\n  }\n}";

    private const string VehiclesInfo =
        "{\n  \"message\": \"ok\",\n  \"result\": {\n    \"properties\": {\n      \"created\": \"2025-09-14T00:12:09.509Z\",\n      \"edited\": \"2025-09-14T00:12:09.509Z\",\n      \"consumables\": \"2 months\",\n      \"name\": \"Sand Crawler\",\n      \"cargo_capacity\": \"50000\",\n      \"passengers\": \"30\",\n      \"max_atmosphering_speed\": \"30\",\n      \"crew\": \"46\",\n      \"length\": \"36.8 \",\n      \"model\": \"Digger Crawler\",\n      \"cost_in_credits\": \"150000\",\n      \"manufacturer\": \"Corellia Mining Corporation\",\n      \"vehicle_class\": \"wheeled\",\n      \"pilots\": [],\n      \"films\": [\n        \"https://www.swapi.tech/api/films/1\",\n        \"https://www.swapi.tech/api/films/5\"\n      ],\n      \"url\": \"https://www.swapi.tech/api/vehicles/4\"\n    },\n    \"_id\": \"5f63a160cf50d100047f97fc\",\n    \"description\": \"A vehicle\",\n    \"uid\": \"4\",\n    \"__v\": 2\n  },\n  \"apiVersion\": \"1.0\",\n  \"timestamp\": \"2025-09-14T06:43:45.350Z\",\n  \"support\": {\n    \"contact\": \"admin@swapi.tech\",\n    \"donate\": \"https://www.paypal.com/donate/?business=2HGAUVTWGR5T2&no_recurring=0&item_name=Support+Swapi+and+keep+the+galaxy%27s+data+free%21+Your+donation+fuels+open-source+innovation+and+helps+us+grow.+Thank+you%21+%F0%9F%9A%80&currency_code=USD\",\n    \"partnerDiscounts\": {\n      \"saberMasters\": {\n        \"link\": \"https://www.swapi.tech/partner-discount/sabermasters-swapi\",\n        \"details\": \"Use this link to automatically get $10 off your purchase!\"\n      },\n      \"heartMath\": {\n        \"link\": \"https://www.heartmath.com/ryanc\",\n        \"details\": \"Looking for some Jedi-like inner peace? Take 10% off your heart-brain coherence tools from the HeartMath Institute!\"\n      }\n    }\n  },\n  \"social\": {\n    \"discord\": \"https://discord.gg/zWvA6GPeNG\",\n    \"reddit\": \"https://www.reddit.com/r/SwapiOfficial/\",\n    \"github\": \"https://github.com/semperry/swapi/blob/main/CONTRIBUTORS.md\"\n  }\n}";

    private const string StarshipsInfo =
        "{\n  \"message\": \"ok\",\n  \"result\": {\n    \"properties\": {\n      \"created\": \"2025-09-14T00:12:09.510Z\",\n      \"edited\": \"2025-09-14T00:12:09.510Z\",\n      \"consumables\": \"1 year\",\n      \"name\": \"CR90 corvette\",\n      \"cargo_capacity\": \"3000000\",\n      \"passengers\": \"600\",\n      \"max_atmosphering_speed\": \"950\",\n      \"crew\": \"30-165\",\n      \"length\": \"150\",\n      \"model\": \"CR90 corvette\",\n      \"cost_in_credits\": \"3500000\",\n      \"manufacturer\": \"Corellian Engineering Corporation\",\n      \"pilots\": [],\n      \"MGLT\": \"60\",\n      \"starship_class\": \"corvette\",\n      \"hyperdrive_rating\": \"2.0\",\n      \"films\": [\n        \"https://www.swapi.tech/api/films/1\",\n        \"https://www.swapi.tech/api/films/3\",\n        \"https://www.swapi.tech/api/films/6\"\n      ],\n      \"url\": \"https://www.swapi.tech/api/starships/2\"\n    },\n    \"_id\": \"5f63a34fee9fd7000499be1e\",\n    \"description\": \"A Starship\",\n    \"uid\": \"2\",\n    \"__v\": 2\n  },\n  \"apiVersion\": \"1.0\",\n  \"timestamp\": \"2025-09-14T06:44:25.266Z\",\n  \"support\": {\n    \"contact\": \"admin@swapi.tech\",\n    \"donate\": \"https://www.paypal.com/donate/?business=2HGAUVTWGR5T2&no_recurring=0&item_name=Support+Swapi+and+keep+the+galaxy%27s+data+free%21+Your+donation+fuels+open-source+innovation+and+helps+us+grow.+Thank+you%21+%F0%9F%9A%80&currency_code=USD\",\n    \"partnerDiscounts\": {\n      \"saberMasters\": {\n        \"link\": \"https://www.swapi.tech/partner-discount/sabermasters-swapi\",\n        \"details\": \"Use this link to automatically get $10 off your purchase!\"\n      },\n      \"heartMath\": {\n        \"link\": \"https://www.heartmath.com/ryanc\",\n        \"details\": \"Looking for some Jedi-like inner peace? Take 10% off your heart-brain coherence tools from the HeartMath Institute!\"\n      }\n    }\n  },\n  \"social\": {\n    \"discord\": \"https://discord.gg/zWvA6GPeNG\",\n    \"reddit\": \"https://www.reddit.com/r/SwapiOfficial/\",\n    \"github\": \"https://github.com/semperry/swapi/blob/main/CONTRIBUTORS.md\"\n  }\n}";

    [Test]
    public void GetLukeSkywalkerInfo_ReturnsSuccessfullyParsedInfo_WhenDataIsCorrect()
    {
        var logger = new Mock<ILogger<LukeSkywalkerProcessor>>();
        var configuration = new Mock<IConfiguration>();
        var endpointSection = new Mock<IConfigurationSection>();
        endpointSection.Setup(x => x.Value).Returns(apiUrl);
        var resultFileSection = new Mock<IConfigurationSection>();
        resultFileSection.Setup(x => x.Value).Returns(fileName);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("Endpoint"))))
            .Returns(endpointSection.Object);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("File"))))
            .Returns(resultFileSection.Object);


        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler.When("*/people/*").Respond(mediaType, LukeInfo);
        httpMessageHandler.When("*/films/*").Respond(mediaType, FilmInfo);
        httpMessageHandler.When("*/vehicles/*").Respond(mediaType, VehiclesInfo);
        httpMessageHandler.When("*/starships/*").Respond(mediaType, StarshipsInfo);
        var httpClient = new HttpClient(httpMessageHandler);

        var sut = new LukeSkywalkerProcessor(logger.Object, configuration.Object, httpClient);

        var result = sut.GetLukeSkywalkerInfo();

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.True(result.Result.Contains(expectedFilmTitle));
        Assert.True(result.Result.Contains(expectedStarshipName));
        Assert.True(result.Result.Contains(expectedVehicleName));
    }

    [Test]
    public void GetLukeSkywalkerInfo_FailsGracefully_WhenUrlReturnsUnsuccessfulStatus()
    {
        var expectedError = "Failed to query Luke Skywalker info";
        var logger = new Mock<ILogger<LukeSkywalkerProcessor>>();
        var configuration = new Mock<IConfiguration>();
        var endpointSection = new Mock<IConfigurationSection>();
        endpointSection.Setup(x => x.Value).Returns(apiUrl);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("Endpoint"))))
            .Returns(endpointSection.Object);

        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler.When("*").Respond(HttpStatusCode.BadRequest);
        var httpClient = new HttpClient(httpMessageHandler);

        var sut = new LukeSkywalkerProcessor(logger.Object, configuration.Object, httpClient);

        Task<string> result = null;
        Action action = () => result = sut.GetLukeSkywalkerInfo();

        Assert.That(action, Throws.Nothing);
        Assert.NotNull(result);
        Assert.That(() => result.Result.Contains(expectedError));
    }

    [Test]
    public void GetLukeSkywalkerInfo_FailsGracefully_WhenNetworkError()
    {
        var expectedError = "Network Error";
        var logger = new Mock<ILogger<LukeSkywalkerProcessor>>();
        var configuration = new Mock<IConfiguration>();
        var endpointSection = new Mock<IConfigurationSection>();
        endpointSection.Setup(x => x.Value).Returns(apiUrl);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("Endpoint"))))
            .Returns(endpointSection.Object);

        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler.When("*").Throw(new Exception(expectedError));
        var httpClient = new HttpClient(httpMessageHandler);

        var sut = new LukeSkywalkerProcessor(logger.Object, configuration.Object, httpClient);

        Task<string> result = null;
        Action action = () => result = sut.GetLukeSkywalkerInfo();

        Assert.That(action, Throws.Nothing);
        Assert.NotNull(result);
        Assert.That(() => result.Result.Contains(expectedError));
    }

    [Test]
    public void GetLukeSkywalkerInfo_FailsGracefully_WhenResponseIsEmpty()
    {
        const string expectedError = "does not contain any JSON tokens";
        var logger = new Mock<ILogger<LukeSkywalkerProcessor>>();
        var configuration = new Mock<IConfiguration>();
        var endpointSection = new Mock<IConfigurationSection>();
        endpointSection.Setup(x => x.Value).Returns(apiUrl);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("Endpoint"))))
            .Returns(endpointSection.Object);

        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler.When("*/people/*").Respond(mediaType, string.Empty);
        var httpClient = new HttpClient(httpMessageHandler);

        var sut = new LukeSkywalkerProcessor(logger.Object, configuration.Object, httpClient);

        Task<string> result = null;
        Action action = () => result = sut.GetLukeSkywalkerInfo();

        Assert.That(action, Throws.Nothing);
        Assert.NotNull(result);
        Assert.That(() => result.Result.Contains(expectedError));
    }

    [Test]
    public void GetLukeSkywalkerInfo_FailsGracefully_WhenResponseIsMalformed()
    {
        const string expectedError = "invalid start of a value";
        var logger = new Mock<ILogger<LukeSkywalkerProcessor>>();
        var configuration = new Mock<IConfiguration>();
        var endpointSection = new Mock<IConfigurationSection>();
        endpointSection.Setup(x => x.Value).Returns(apiUrl);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("Endpoint"))))
            .Returns(endpointSection.Object);

        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler.When("*/people/*").Respond(mediaType, LukeInfo.Substring(10));
        var httpClient = new HttpClient(httpMessageHandler);

        var sut = new LukeSkywalkerProcessor(logger.Object, configuration.Object, httpClient);

        Task<string> result = null;
        Action action = () => result = sut.GetLukeSkywalkerInfo();

        Assert.That(action, Throws.Nothing);
        Assert.NotNull(result);
        Assert.That(() => result.Result.Contains(expectedError));
    }

    [Test]
    public void GetLukeSkywalkerInfo_DoesNotReplaceUrls_WhenReturnedContentIsEmpty()
    {
        string[] expectedErrors = ["No Films found", "No Vehicles found", "No Starships found"];
        var logger = new Mock<ILogger<LukeSkywalkerProcessor>>();
        var configuration = new Mock<IConfiguration>();
        var endpointSection = new Mock<IConfigurationSection>();
        endpointSection.Setup(x => x.Value).Returns(apiUrl);
        var resultFileSection = new Mock<IConfigurationSection>();
        resultFileSection.Setup(x => x.Value).Returns(fileName);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("Endpoint"))))
            .Returns(endpointSection.Object);
        configuration.Setup(x => x.GetSection(It.Is<string>(s => s.Contains("File"))))
            .Returns(resultFileSection.Object);


        var httpMessageHandler = new MockHttpMessageHandler();
        httpMessageHandler.When("*/people/*").Respond(mediaType, LukeInfo);
        httpMessageHandler.When("*/films/*").Respond(HttpStatusCode.NotFound);
        httpMessageHandler.When("*/vehicles/*").Respond(HttpStatusCode.NotFound);
        httpMessageHandler.When("*/starships/*").Respond(HttpStatusCode.NotFound);
        var httpClient = new HttpClient(httpMessageHandler);

        var sut = new LukeSkywalkerProcessor(logger.Object, configuration.Object, httpClient);

        var result = sut.GetLukeSkywalkerInfo();

        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.True(!result.Result.Contains(expectedFilmTitle));
        Assert.True(!result.Result.Contains(expectedStarshipName));
        Assert.True(!result.Result.Contains(expectedVehicleName));

        foreach (var error in expectedErrors)
            logger.Verify(x => x.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((
                        v,
                        t) => v.ToString().Contains(error)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
    }
}