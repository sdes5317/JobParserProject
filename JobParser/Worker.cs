using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobParser.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JobParser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ParserService _parserService;
        private readonly RemoteRepository _remoteRepository;
        private readonly IMapper _mapper;
        private readonly int _interval;
        private DateTime ?_lastPublishTime;

        public Worker(
            ILogger<Worker> logger, 
            ParserService parserService,
            RemoteRepository remoteRepository,
            IMapper mapper)
        {
            _logger = logger;
            _parserService = parserService;
            _remoteRepository = remoteRepository;
            _mapper = mapper;
            _interval = 2 * 60 * 60 * 1000;//2 Hour * 60 Min * 60 Sec * 1000Ms
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running");
                try
                {
                    //todo 使用排程器套件重構
                    _logger.LogInformation(nameof(FindNewJobTask));
                    await FindNewJobTask();
                    _logger.LogInformation(nameof(PublishJobsTask));
                    await PublishJobsTask();

                    _logger.LogInformation("Worker Finish");
                }
                catch(Exception e)
                {
                    _logger.LogError("Worker Get Error");
                    _logger.LogError(e.ToString());
                }
                finally
                {
                    if(_parserService != null) await _parserService.CloseAsync();
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
        /// <summary>
        /// 每天早上定時發送新職缺訊息
        /// </summary>
        /// <returns></returns>
        private async Task PublishJobsTask()
        {
            if (_lastPublishTime is null || _lastPublishTime.Value.Date != DateTime.Today)
            {
                await _remoteRepository.PublishYesterdayJobToUser();
                _lastPublishTime = DateTime.Today;
            }
        }

        private async Task FindNewJobTask()
        {
            await _parserService.InitBrowser();
            await _parserService.Login();
            await _parserService.GetJobPageElement();
            var jobElements = (await _parserService.GetJobElement()).ToList();
            var companyElements = (await _parserService.GetCompanyElement()).ToList();
            var areaElements = (await _parserService.GetAreaElement()).ToList();

            var jobDtos = new List<JobDto>();
            for (int i = 0; i < jobElements.Count(); i++)
            {
                var detail = new JobDto();
                _mapper.Map(jobElements[i], detail);
                _mapper.Map(companyElements[i], detail);
                _mapper.Map(areaElements[i], detail);
                jobDtos.Add(detail);
            }

            await _remoteRepository.UpdateNewJobs(jobDtos);
        }
    }
}
