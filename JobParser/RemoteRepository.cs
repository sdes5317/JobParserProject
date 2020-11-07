using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JobParser
{
    public class RemoteRepository
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string Url;

        public RemoteRepository(string url)
        {
            Url = url;
        }

        public async Task<HttpResponseMessage> SendNewJobs(IEnumerable<JobDto> jobDtos)
        {
            var apiName = "Jobs/InsertPerson";
            var stringContext = new StringContent(JsonConvert.SerializeObject(jobDtos), Encoding.UTF8, "application/json");
            return await HttpClient.PostAsync(Url + apiName, stringContext);
        }
    }
}
