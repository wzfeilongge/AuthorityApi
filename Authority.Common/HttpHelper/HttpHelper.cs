using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authority.Common.HttpHelper
{
    public static class HttpHelper
    {
        #region PostAsync
        public  async static Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            if (request==null) {
                return default;
            }
            var obj = JsonConvert.SerializeObject(request);
            var content = new StringContent(obj);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var resp =  await client.PostAsync(url, content);
            var body = await resp.Content.ReadAsStringAsync();
            client.Dispose();
            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        #endregion

        #region PutAsync
        public  async static Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request)
        {
            if (request == null)
            {
                return default;
            }
            var obj = JsonConvert.SerializeObject(request);
            var content = new StringContent(obj);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var resp = await client.PutAsync(url, content);
            var body = await resp.Content.ReadAsStringAsync();
            client.Dispose();
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        #endregion

        #region DeleteAsync
        public  async static Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            var client = new HttpClient();
            var resp = await client.DeleteAsync(url);
            var body = await resp.Content.ReadAsStringAsync();         
            client.Dispose();
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        #endregion

        #region GetAsync
        public async static Task<TResponse> GetAsync<TRequest, TResponse>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            StringBuilder urlSb = new StringBuilder();
            urlSb.Append("?");
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null && p.GetValue(request).ToString() != "0")
                {
                    urlSb.Append(p.Name).Append("=").Append(p.GetValue(request).ToString()).Append("&");
                }
            }

            url = (url + urlSb.ToString()).TrimEnd('&');
            var client = new HttpClient();
            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            client.Dispose();
            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        #endregion

        #region GetAsync 无参
        public async static Task<TResponse> GetAsync<TResponse>(string url)
        {          
            var client = new HttpClient();
            var resp =await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();          
            client.Dispose();
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        #endregion

    }


}
