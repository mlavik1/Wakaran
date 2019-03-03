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

            MicrosoftTranslationService transService = new MicrosoftTranslationService();
            TranslationServiceResult transServiceResult = transService.GetTranslation(inSearchText, inLanguage);

            MicrosoftTransliterationService translitService = new MicrosoftTransliterationService();
            TransliterationServiceResult translitServiceResult = translitService.GetTransliteration(inSearchText, inLanguage);

            translationResult.mSourceText = inSearchText;
            translationResult.mTranslitText = transServiceResult != null ? transServiceResult.translation : "TRANSLATION FAILED";
            translationResult.mTranslatedText = translitServiceResult != null ? translitServiceResult.transliteration : "";
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
