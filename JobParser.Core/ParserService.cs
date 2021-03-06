﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace JobParser.Core
{
    public class ParserService
    {
        private Browser _browser;
        private Page _page;
        private readonly string _loginUserName;
        private readonly string _loginPassword;

        public ParserService(string loginUserName, string loginPassword)
        {
            _loginUserName = loginUserName;
            _loginPassword = loginPassword;
        }

        public async Task InitBrowserAsync()
        {
#if DEBUG
            await DownloadBrowser();
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
            });
#else
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
            
                            ExecutablePath = "/usr/bin/chromium-browser",
                            Headless = true,
                            Args = new[] { "--no-sandbox" }
                        });
#endif

        }

        public async Task CloseAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
            }
        }
        public async Task DownloadBrowser()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        }

        public async Task LoginAsync()
        {
            var page = await _browser.NewPageAsync();
            await page.SetViewportAsync(new ViewPortOptions());
            await page.GoToAsync("https://pda.104.com.tw/my104/mate/list?itemNo=x2&u=qj9dzg");
            await page.WaitForSelectorAsync("#username");
            await page.TypeAsync("#username", _loginUserName);
            await page.TypeAsync("#password", _loginPassword);
            await page.ClickAsync("#submitBtn");
            await page.WaitForNavigationAsync();
            await page.CloseAsync();
        }

        public async Task GetJobPageElementAsync()
        {
            _page = await _browser.NewPageAsync();
            await _page.SetViewportAsync(new ViewPortOptions());
            await _page.GoToAsync("https://pda.104.com.tw/my104/mate/list?itemNo=x2&u=qj9dzg");
        }

        public async Task<IEnumerable<JobElement>> GetJobElementAsync()
        {
            var element = await _page.QuerySelectorAllAsync(".joblist_cont .jobname_1");
            var jobElements = new List<JobElement>();
            foreach (var elementHandle in element)
            {
                jobElements.Add(new JobElement()
                {
                    innerText = (await elementHandle.GetPropertyAsync(nameof(JobElement.innerText))).RemoteObject.Value.ToString(),
                    innerHTML = (await (await elementHandle.GetPropertyAsync("firstElementChild")).GetPropertyAsync("href")).RemoteObject.Value.ToString(),
                });
            }

            return jobElements;
        }

        public async Task<IEnumerable<CompanyElement>> GetCompanyElementAsync()
        {
            var element = await _page.QuerySelectorAllAsync(".joblist_cont .compname_1");
            var jobElements = new List<CompanyElement>();
            foreach (var elementHandle in element)
            {
                jobElements.Add(new CompanyElement()
                {
                    innerText = (await elementHandle.GetPropertyAsync(nameof(CompanyElement.innerText))).RemoteObject.Value.ToString(),
                });
            }

            return jobElements;
        }

        public async Task<IEnumerable<AreaElement>> GetAreaElementAsync()
        {
            var element = await _page.QuerySelectorAllAsync(".joblist_cont .area");
            var jobElements = new List<AreaElement>();
            foreach (var elementHandle in element)
            {
                jobElements.Add(new AreaElement()
                {
                    innerText = (await elementHandle.GetPropertyAsync(nameof(AreaElement.innerText))).RemoteObject.Value.ToString(),
                });
            }

            return jobElements;
        }
    }
}