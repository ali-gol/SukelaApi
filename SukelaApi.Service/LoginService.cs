using HtmlAgilityPack;
using SukelaApi.Library;
using SukelaApi.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SukelaApi.Service
{
    public class LoginService
    {
        private CustomWebClient GetClient(string url)
        {
            var client = new CustomWebClient(url);
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            client.Headers.Add("origin", "https://eksisozluk.com");
            client.Headers.Add("upgrade-insecure-requests", "1");
            client.Headers.Add("x-requested-with", "XMLHttpRequest");
            return client;
        }
        public async Task<BaseResponse<string>> Login(string username, string password)
        {
            var getClient = GetClient("https://eksisozluk.com/giris");

            var getResult = await getClient.DownloadStringTaskAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(getResult);
            var verificationCode = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[2]/div[2]/div[2]/section[1]/div[1]/form[1]/input[1]").GetAttributeValue("value", null);

            var postClient = GetClient("https://eksisozluk.com/giris");
            postClient.RequestCookies = getClient.ResponseCookies;
            getClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            try
            {
                var result = await postClient.UploadStringTaskAsync($"__RequestVerificationToken={verificationCode}&UserName={username}&Password={password}&RememberMe=true");
            }
            catch (System.Net.WebException ex)
            {
                var response = (System.Net.HttpWebResponse)ex.Response;
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new BaseResponse<string> { IsSuccess = false, Message = "Invalid user or password" };
                }
                throw ex;
            }
            var loginCookies = postClient.ResponseCookies;
            return new BaseResponse<string> { IsSuccess = true, Message = "Login Success", Data = string.Join(';', getClient.ResponseCookies.GetCookies(new Uri("https://eksisozluk.com"))) };
        }
    }
}
