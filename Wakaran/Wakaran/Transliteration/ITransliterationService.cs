
namespace Wakaran
{
    public interface ITransliterationService
    {
        TransliterationServiceResult GetTransliteration(string inSourceString, Language inLanguage);
    }
}
