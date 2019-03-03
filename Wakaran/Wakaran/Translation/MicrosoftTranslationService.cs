using System;
using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wakaran
{
    public class MicrosoftTranslationService : ITranslationService
    {
        const string subscriptionKey = "dcd615071a0245c29d0545ff5385651b";

        public TranslationServiceResult GetTranslation(string inSourceString, Language inLanguage)
        {
            string host = "https://api.cognitive.microsofttranslator.com";
            string route = "/translate?api-version=3.0&from=" + GetLanguageString(inLanguage) + "&to=en";

            System.Object[] body = new System.Object[] { new { Text = inSourceString } };
            string requestBody = JsonConvert.SerializeObject(body);
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();

            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(host + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            TranslationServiceResult transRes = null;
            try
            {
                // Send request, get response
                HttpResponseMessage response = client.SendAsync(request).Result;
                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Parse response JSON
                JArray jArr = JArray.Parse(jsonResponse);
                JToken jToken = jArr[0];
                JToken transToken = jToken["translations"];
                string transText = ((JArray)transToken)[0]["text"].ToString();
                transRes = new TranslationServiceResult();
                transRes.translation = transText;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return transRes;
        }

        private string GetLanguageString(Language inLanguage)
        {
            switch(inLanguage)
            {
                case Language.English:
                    return "en";
                case Language.Japanese:
                    return "ja";
                case Language.Chinese:
                    return "zh-Hans";
            }
            throw new NotImplementedException();
        }
    }
}
