using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wakaran
{
    class TranslationRresult
    {
        public string mSourceText;
        public string mTranslatedText;
        public string mTranslitText;
        public Language mSourceLanguage;
        public Language mTargetLanguage;
    }

    class TranslationHelper
    {
        public TranslationRresult GetTranslation(string inSearchText, Language inLanguage)
        {
            TranslationRresult translationResult = new TranslationRresult();

            bool textIsANSI = true;
            foreach (char c in inSearchText)
            {
                if (c >= 0x00ff)
                {
                    textIsANSI = false;
                    break;
                }
            }

            translationResult.mSourceLanguage = textIsANSI ? Language.English : inLanguage;
            translationResult.mTargetLanguage = textIsANSI ? inLanguage : Language.English;

            string sourceLanguageCode = GetGoogleLanguageCode(translationResult.mSourceLanguage);
            string targetLanguageCode = GetGoogleLanguageCode(translationResult.mTargetLanguage);

            string languageSearchCode = sourceLanguageCode + "|" + targetLanguageCode;

            string googleTranslateURL = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", inSearchText, languageSearchCode);

            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            if (inLanguage == Language.Chinese)
            {
                // Google translate uses iso-8859-1 for Chinese pinyin encoding.
                encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            }
            else
            {
                encoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            }

            WebClient webClient = new WebClient();
            webClient.Encoding = encoding;

            string googleResult;
            try
            {
                byte[] htmlData = webClient.DownloadData(googleTranslateURL);
                googleResult = encoding.GetString(htmlData);
            }
            catch (Exception ex)
            {
                // TODO: Show message
                return translationResult;
            }

            mshtml.HTMLDocument doc = new mshtml.HTMLDocument();
            mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)doc;
            doc2.write(googleResult);

            mshtml.IHTMLElement elemResultBox = doc.getElementById("result_box");
            mshtml.IHTMLElement elemTranslit = doc.getElementById("src-translit");

            string txtResultBox = elemResultBox.innerText;
            string txtTranslit = elemTranslit.innerText;

            translationResult.mSourceText = inSearchText;
            translationResult.mTranslitText = txtTranslit;
            translationResult.mTranslatedText = txtResultBox;
            return translationResult;
        }

        private string GetGoogleLanguageCode(Language inLanguage)
        {
            switch (inLanguage)
            {
                case Language.English:
                    return "en";
                    break;
                case Language.Japanese:
                    return "ja";
                    break;
                case Language.Chinese:
                    return "cn";
                    break;
            }
            return "NONE";
        }
    }
}
