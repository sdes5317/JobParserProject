using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using JobWebApi.Repository;
using JobWebApi.Repository.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JobWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobsController : ControllerBase
    {
        private MySqlRepository _repository;
        private readonly ILogger<JobsController> _logger;
        private readonly IMapper _mapper;
        private readonly string _lineNotifyKey;


        public JobsController(ILogger<JobsController> logger, MySqlRepository repository, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _lineNotifyKey = configuration.GetSection("LineNotifyKey").Value;
        }

        [HttpGet]
        public IEnumerable<JobDetail> GetTodayJobs()
        {
            return _repository.JobContext.JobDetails.Where(detail => detail.CreationTime.Date == DateTime.Today);
        }

        [HttpPut]
        public void SetJob(JobDetail jobDetail)
        {
            _repository.JobContext.JobDetails.Update(jobDetail);
            _repository.JobContext.SaveChanges();
        }
        [HttpPost]
        public void InsertJobs(IEnumerable<JobDetail> jobDtos)
        {
            foreach (var jobDto in jobDtos)
            {
                var count = _repository.JobContext.JobDetails.Count(jobDao =>
                    jobDao.Company == jobDto.Company && jobDao.Name == jobDto.Name);

                if (count == 0)
                {
                    var dao = _mapper.Map<JobDetail>(jobDto);
                    _repository.JobContext.JobDetails.AddAsync(dao);
                    _repository.JobContext.SaveChanges();
                }
            }
        }
        [HttpGet]
        public async Task PublishYesterdayJobToUser()
        {
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today;
            var jobToSend = _repository.JobContext.JobDetails.Where(detail =>
                detail.CreationTime.Date >= startDate &&
                detail.CreationTime.Date < endDate).ToList();

            await SendLineNotify(jobToSend);
        }

        private Task<HttpResponseMessage> SendLineNotify(IEnumerable<JobDetail> jobToSend)
        {
            //todo 把Line發送功能抽成模組
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization",
                $"Bearer {_lineNotifyKey}");

            var context = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>>()
                {
                    //todo 針對發送的訊息進行排版
                    new KeyValuePair<string, string>("message", JsonConvert.SerializeObject(jobToSend,Formatting.Indented))
                });

            return httpClient.PostAsync(
                 "https://notify-api.line.me/api/notify", context);
        }
    }
}
