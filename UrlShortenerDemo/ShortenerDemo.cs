using BitlyAPI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerDemo
{
    public class ShortenerDemo
    {
        private readonly string _genericAccessToken;
        public ShortenerDemo()
        {
            //_genericAccessToken = 
            //read json in
            using (StreamReader file = File.OpenText(@"./appsettings.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject appsettings = (JObject)JToken.ReadFrom(reader);
                    _genericAccessToken = appsettings["genericAccessToken"].ToString();
                }
            }

            
        }

        public async Task Run()
        {
            var longUrl = $@"https://www.google.com/maps/search/48.857419,2.294905/";
            var tiny = MakeTinyUrl(longUrl);
            var bitly = await MakeBitlyUrl(longUrl);
        }

        //shamelessly stolen from: https://stackoverflow.com/questions/366115/using-tinyurl-com-in-a-net-application-possible
        public string MakeTinyUrl(string url)
        {
            try
            {
                if (url.Length <= 30)
                {
                    return url;
                }
                if (!url.ToLower().StartsWith("http") && !url.ToLower().StartsWith("ftp"))
                {
                    url = "http://" + url;
                }
                var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + url);
                var res = request.GetResponse();
                string text;
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception)
            {
                return url;
            }
        }

        //bitly example: https://github.com/doublej42/BitlyAPI
        public async Task<string> MakeBitlyUrl(string url)
        {
            var bitly = new Bitly(_genericAccessToken);
            var linkResponse = await bitly.PostShorten(url);
            var newLink = linkResponse.Link;

            return newLink;
        }
    }
}
