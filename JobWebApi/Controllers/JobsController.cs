using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using JobWebApi.Repository;
using JobWebApi.Repository.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobsController : ControllerBase
    {
        private MySqlRepository _repository;
        private readonly ILogger<JobsController> _logger;
        private readonly IMapper _mapper;


        public JobsController(ILogger<JobsController> logger, MySqlRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<JobDetail> GetTodayJobs()
        {
            return _repository.JobContext.JobDetails.Where(detail => detail.CreationTime.Date == DateTime.Today);
        }

        [HttpPut]
        public void SetPerson(JobDetail jobDetail)
        {
            _repository.JobContext.JobDetails.Update(jobDetail);
            _repository.JobContext.SaveChanges();
        }
        [HttpPost]
        public void InsertPerson(IEnumerable<JobDetail> jobDtos)
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
    }
}
