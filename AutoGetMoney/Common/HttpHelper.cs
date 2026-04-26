using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Documents;

namespace AutoGetMoney.Common
{
    public enum ContentType
    {
        Json,
        Xml
    }

    public enum HttpMethodType
    {
        Get,
        Post
    }

    public static class HttpHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static void SetAuthorization(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static async Task<string> SendAsync(Model.Section? ClsClickSection, string url, object? body = null, HttpMethodType method = HttpMethodType.Post, ContentType contentType = ContentType.Json, Dictionary<string, string>? customHeaders = null)
        {
            try
            {
                if (ClsClickSection != null)
                {
                    await ClsClickSection.WaitIfApiLimitExceededAsync();
                }

                HttpRequestMessage request = method == HttpMethodType.Get
                    ? new HttpRequestMessage(HttpMethod.Get, url)
                    : new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = contentType switch
                        {
                            ContentType.Json => new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"),
                            ContentType.Xml => new StringContent(SerializeToXml(body!), Encoding.UTF8, "application/xml"),
                            _ => throw new NotSupportedException("Unknown content type")
                        }
                    };

                // 공통 헤더
                if (customHeaders != null)
                {
                    foreach (var header in customHeaders)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"[ERROR] {ex.Message}";
            }
        }


        private static string SerializeToXml(object obj)
        {
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }
    }
}

//JSON + POST
//var result = await HttpHelper.SendAsync("https://api.example.com/user", new
//{
//    name = "홍길동",
//    age = 30
//}, HttpMethodType.Post, ContentType.Json);

//XML + POST
//var result = await HttpHelper.SendAsync("https://api.example.com/user", new
//{
//    name = "홍길동",
//    age = 30
//}, HttpMethodType.Post, ContentType.Xml);

//GET 요청
//var result = await HttpHelper.SendAsync("https://api.example.com/data", null, HttpMethodType.Get);

//응답 파싱(JSON)
//var parsed = JsonSerializer.Deserialize<MyResponseModel>(result);

//응답 파싱(XML)
//using var reader = new StringReader(result);
//var serializer = new XmlSerializer(typeof(MyResponseModel));
//var parsed = (MyResponseModel)serializer.Deserialize(reader);


