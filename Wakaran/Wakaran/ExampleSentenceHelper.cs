using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wakaran
{
    class ExampleSentence
    {
        public string mSourceText;
        public string mTargetText;
    }

    class ExampleSearchResult
    {
        public List<ExampleSentence> mExampleSentences = new List<ExampleSentence>();
    }

    class ExampleSentenceHelper
    {
        public ExampleSearchResult GetExampleSentences(string inSearchText, Language inLanguage)
        {
            ExampleSearchResult exampleSearchResult = new ExampleSearchResult();

            List<string> sourceTexts = new List<string>();
            List<string> translatedTexts = new List<string>();

            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;

            string weblioURL = String.Format("https://{0}.weblio.jp/sentence/content/{1}", inLanguage == Language.Japanese ? "ejje" : "cjjc", inSearchText);
            string weblioResult = "";

            try
            {
                weblioResult = webClient.DownloadString(weblioURL);

            }
            catch (Exception ex)
            {
                // TODO: show message
                return exampleSearchResult;
            }

            string sourceTextClassName = inLanguage == Language.Japanese ? "CJJ" : "CC";
            string targetTextClassName = inLanguage == Language.Japanese ? "CJE" : "CJ";

            Regex r = new Regex(Regex.Escape(String.Format("<p class=qot{0}>", sourceTextClassName)) + "(.*?)" + Regex.Escape("</p>"));
            MatchCollection matches = r.Matches(weblioResult);
            foreach (Match match in matches)
            {
                string str = match.Groups[1].Value;
                int iSpan = str.IndexOf("<span");
                if (iSpan != -1)
                {
                    str = str.Substring(0, iSpan);
                }
                str = str.Replace("<b>", "").Replace("</b>", "");
                sourceTexts.Add(str);
            }

            r = new Regex(Regex.Escape(String.Format("<p class=qot{0}>", targetTextClassName)) + "(.*?)" + Regex.Escape("</p>"));
            matches = r.Matches(weblioResult);
            foreach (Match match in matches)
            {
                string str = match.Groups[1].Value;
                int startIndex = str.IndexOf('<');
                int i;
                while (startIndex != -1)
                {
                    int bracketCount = 0;
                    for (i = startIndex + 1; i < str.Length; i++)
                    {
                        if (str[i] == '<')
                            bracketCount++;
                        else if (str[i] == '>')
                        {
                            if (bracketCount == 0)
                                break;
                            bracketCount--;
                        }
                    }
                    str = str.Remove(startIndex, i - startIndex + 1);
                    str = Regex.Replace(str, "<span>(.*?)</span>", "");
                    str = Regex.Replace(str, "&(.*?);", "");
                    startIndex = str.IndexOf('<');
                }
                translatedTexts.Add(str);
            }

            for (int i = 0; i < sourceTexts.Count() && i < translatedTexts.Count(); i++)
            {
                ExampleSentence exampleSentence = new ExampleSentence();
                exampleSentence.mSourceText = sourceTexts[i];
                exampleSentence.mTargetText = translatedTexts[i];
                exampleSearchResult.mExampleSentences.Add(exampleSentence);
            }

            return exampleSearchResult;
        }
    }
}
