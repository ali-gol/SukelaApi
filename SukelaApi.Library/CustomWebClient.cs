using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SukelaApi.Library
{
    public class CustomWebClient
    {
        private readonly Uri _url;
        public void SetCookieString(string cookieString)
        {
            CookieContainer cookiecontainer = new CookieContainer();
            string[] cookies = cookieString.Split(';');
            foreach (string cookie in cookies)
            {
                var c = cookie.Split('=');
                cookiecontainer.Add(new Cookie(c[0], c[1], "/", _url.Host));
                //cookiecontainer.SetCookies(new Uri($"{_url.Scheme}://{_url.Host}"), cookie);
            }
            RequestCookies = cookiecontainer;
        }
        public CookieContainer RequestCookies { get; set; } = new CookieContainer();
        public CookieContainer ResponseCookies { get; set; } = new CookieContainer();
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public List<Cookie> GetRequestCookies()
        {
            return RequestCookies.GetCookies(_url).Cast<Cookie>().ToList();
        }
        public List<Cookie> GetResponseCookies()
        {
            return ResponseCookies.GetCookies(_url).Cast<Cookie>().ToList();
        }
        public CustomWebClient(string url)
        {
            this._url = new Uri(url);
        }
        public Cookie GetCookie(string key)
        {
            var cookie = ResponseCookies.GetCookies(_url).Cast<Cookie>().FirstOrDefault(t => t.Name.Contains(key));
            return cookie;
        }
        public async Task<string> DownloadStringTaskAsync()
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            foreach (var item in Headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }
            request.Method = "GET";
            request.CookieContainer = RequestCookies ?? new CookieContainer();
            var response = (HttpWebResponse)await request.GetResponseAsync();
            foreach (Cookie item in response.Cookies)
            {
                ResponseCookies.Add(item);
            }
            using var dataStream = response.GetResponseStream();
            using var reader = new StreamReader(dataStream);
            return await reader.ReadToEndAsync();
        }

        public string GetStringCookies()
        {
            var cookies = ResponseCookies.GetCookies(_url);
            var result = cookies.ToString();
            return result;
        }
        public async Task<string> UploadStringTaskAsync(string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = RequestCookies ?? new CookieContainer();
            if (Headers.ContainsKey("Accept"))
            {
                request.Accept = Headers["Accept"];
                Headers.Remove("Accept");
            }
            if (Headers.ContainsKey("User-Agent"))
            {
                request.UserAgent = Headers["User-Agent"];
                Headers.Remove("User-Agent");
            }
            if (Headers.ContainsKey("Connection"))
            {
                request.Connection = Headers["Connection"];
                Headers.Remove("Connection");
            }
            foreach (var item in Headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            var requestStrem = await request.GetRequestStreamAsync();
            requestStrem.Write(byteArray, 0, byteArray.Length);
            requestStrem.Close();

            var response = (HttpWebResponse)await request.GetResponseAsync();
            foreach (Cookie item in response.Cookies)
            {
                ResponseCookies.Add(item);
            }

            var responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            var result = await reader.ReadToEndAsync();
            return result;

        }
        public string UploadString(string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = RequestCookies ?? new CookieContainer();
            if (Headers.ContainsKey("Accept"))
            {
                request.Accept = Headers["Accept"];
                Headers.Remove("Accept");
            }
            if (Headers.ContainsKey("User-Agent"))
            {
                request.UserAgent = Headers["User-Agent"];
                Headers.Remove("User-Agent");
            }
            if (Headers.ContainsKey("Connection"))
            {
                request.Connection = Headers["Connection"];
                Headers.Remove("Connection");
            }
            foreach (var item in Headers)
            {
                request.Headers.Add(item.Key, item.Value);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;
            using (var requestStrem = request.GetRequestStream())
            {
                requestStrem.Write(byteArray, 0, byteArray.Length);
                requestStrem.Close();
                try
                {
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        foreach (Cookie item in response.Cookies)
                        {
                            ResponseCookies.Add(item);
                        }
                        using (var responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                            var result = reader.ReadToEnd();
                            return result;
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var error = reader.ReadToEnd();
                            throw new Exception(error);
                        }
                    }
                }
            }
        }
    }
}