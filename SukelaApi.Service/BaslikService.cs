using HtmlAgilityPack;
using SukelaApi.Library;
using SukelaApi.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SukelaApi.Service
{
    public class BaslikService
    {
        private readonly string _cookieString;
        public BaslikService(string cookieString)
        {
            _cookieString = cookieString; //"a=2KLomO6SzZQ0PTsptLtToinocQr151HFJ4BLI9DHJ/17kyjkxLQN5SM19fK4hfpZmwblXYO5TPZ8tcCfasiSiJv4hbFmuKxrdhzkjPbswUYggsO6Lp79C5bxSgQbm3zZGg+2m91M5UmFYwACn0yXmw5D8bRTdWXdpKfUZiFYaG4RXVTzQce5IWyg0OrIb7Gy; expires=Thu, 16-Jul-2020 14:23:28 GMT; path=/; HttpOnly";
        }
      
        public Task<List<BaslikModel>> GetBugun()
        {
            return GetTopic("https://eksisozluk.com/basliklar/bugun");
        }
        public Task<List<BaslikModel>> GetGundem()
        {
            return GetTopic("https://eksisozluk.com/basliklar/gundem");
        }

        #region helper methods
        private async Task<List<BaslikModel>> GetTopic(string url)
        {
            var client = GetClient(url);
            client.SetCookieString(_cookieString);
            var html = await client.DownloadStringTaskAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var topics = doc.DocumentNode.SelectSingleNode("/ul[1]").ChildNodes
                .Skip(1)
                .Where(c => c.ChildNodes.Count > 1)
                .Select(c => c.ChildNodes[1])
                .Select(c => new BaslikModel
                {
                    Url = $"https://eksisozluk.com/{c.Attributes["href"].Value}",
                    Title = c.ChildNodes.Count == 1 ? c.InnerText : $"{c.ChildNodes[0].InnerText} ({c.ChildNodes[1].InnerText})"
                }).ToList();
            return topics;
        }
        private CustomWebClient GetClient(string url)
        {
            var client = new CustomWebClient(url);
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            client.Headers.Add("origin", "https://eksisozluk.com");
            client.Headers.Add("upgrade-insecure-requests", "1");
            client.Headers.Add("x-requested-with", "XMLHttpRequest");
            return client;
        }
        #endregion
    }

}
