using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ADE.Tutorial
{
    public class GetTime
    {
        private readonly ILogger logger;

        private static readonly List<TimeZoneInfo> timeZones = new List<TimeZoneInfo>
        {
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
            TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
            TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
            TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
            TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
            TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time")
        };

        public GetTime(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<GetTime>();
        }

        [Function("GetTime")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time")] HttpRequestData req)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var now = DateTime.UtcNow;

            response.WriteString($"UTC Time: {now}");

            foreach (var timeZone in timeZones)
                response.WriteString($"{(timeZone.IsDaylightSavingTime(now) ? timeZone.DaylightName : timeZone.StandardName)}: {TimeZoneInfo.ConvertTimeFromUtc(now, timeZone)}");

            return response;
        }
    }
}
