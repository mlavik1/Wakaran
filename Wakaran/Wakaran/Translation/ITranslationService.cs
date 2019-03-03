
namespace Wakaran
{
    public interface ITranslationService
    {
        TranslationServiceResult GetTranslation(string inSourceString, Language inLanguage);
    }
}
